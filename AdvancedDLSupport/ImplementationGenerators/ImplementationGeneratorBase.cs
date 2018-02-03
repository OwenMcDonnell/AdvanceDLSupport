﻿using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using JetBrains.Annotations;
using Mono.DllMap.Extensions;
using static AdvancedDLSupport.ImplementationOptions;

namespace AdvancedDLSupport.ImplementationGenerators
{
    /// <summary>
    /// Base class for implementation generators.
    /// </summary>
    /// <typeparam name="T">The type of member to generate the implementation for.</typeparam>
    internal abstract class ImplementationGeneratorBase<T> : IImplementationGenerator<T> where T : MemberInfo
    {
        /// <inheritdoc />
        public ImplementationOptions Options { get; }

        /// <summary>
        /// Gets the module in which the implementation should be generated.
        /// </summary>
        [NotNull]
        protected ModuleBuilder TargetModule { get; }

        /// <summary>
        /// Gets the type in which the implementation should be generated.
        /// </summary>
        [NotNull]
        protected TypeBuilder TargetType { get; }

        /// <summary>
        /// Gets the IL generator for the constructor of the type in which the implementation should be generated.
        /// </summary>
        [NotNull]
        protected ILGenerator TargetTypeConstructorIL { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImplementationGeneratorBase{T}"/> class.
        /// </summary>
        /// <param name="targetModule">The module where the implementation should be generated.</param>
        /// <param name="targetType">The type in which the implementation should be generated.</param>
        /// <param name="targetTypeConstructorIL">The IL generator for the target type's constructor.</param>
        /// <param name="options">The configuration object to use.</param>
        protected ImplementationGeneratorBase
        (
            [NotNull] ModuleBuilder targetModule,
            [NotNull] TypeBuilder targetType,
            [NotNull] ILGenerator targetTypeConstructorIL,
            ImplementationOptions options
        )
        {
            TargetModule = targetModule;
            TargetType = targetType;
            TargetTypeConstructorIL = targetTypeConstructorIL;
            Options = options;
        }

        /// <inheritdoc />
        public void GenerateImplementation(T member)
        {
            var metadataAttribute = member.GetCustomAttribute<NativeSymbolAttribute>() ?? new NativeSymbolAttribute(member.Name);
            var symbolName = metadataAttribute.Entrypoint ?? member.Name;

            var uniqueIdentifier = Guid.NewGuid().ToString().Replace("-", "_");
            var memberIdentifier = $"{member.Name}_{uniqueIdentifier}";

            GenerateImplementation(member, symbolName, memberIdentifier);
        }

        /// <summary>
        /// Generates the implementation for the given member using the given symbol name and member identifier.
        /// </summary>
        /// <param name="member">The member to generate the implementation for.</param>
        /// <param name="symbolName">The name of the symbol in the native library.</param>
        /// <param name="uniqueMemberIdentifier">The identifier to use for generated types and methods.</param>
        protected abstract void GenerateImplementation([NotNull] T member, [NotNull] string symbolName, [NotNull] string uniqueMemberIdentifier);

        /// <summary>
        /// Emits a call to <see cref="AnonymousImplementationBase.ThrowIfDisposed"/>.
        /// </summary>
        /// <param name="il">The IL generator.</param>
        protected void EmitDisposalCheck([NotNull] ILGenerator il)
        {
            var throwMethod = typeof(AnonymousImplementationBase).GetMethod("ThrowIfDisposed", BindingFlags.NonPublic | BindingFlags.Instance);

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, throwMethod);
        }

        /// <summary>
        /// Generates a lazy loaded field with the specified value factory.
        /// </summary>
        /// <param name="valueFactory">The value factory to use for the lazy loaded field.</param>
        /// <param name="type">The return type of the lazy field.</param>
        protected void GenerateLazyLoadedField([NotNull] MethodBuilder valueFactory, [NotNull] Type type)
        {
            var funcType = typeof(Func<>).MakeGenericType(type);
            var lazyType = typeof(Lazy<>).MakeGenericType(type);

            var funcConstructor = funcType.GetConstructors().First();
            var lazyConstructor = lazyType.GetConstructors().First
            (
                c =>
                    c.GetParameters().Any() &&
                    c.GetParameters().Length == 1 &&
                    c.GetParameters().First().ParameterType == funcType
            );

            // Use the lambda instead of the function directly.
            TargetTypeConstructorIL.Emit(OpCodes.Ldftn, valueFactory);
            TargetTypeConstructorIL.Emit(OpCodes.Newobj, funcConstructor);
            TargetTypeConstructorIL.Emit(OpCodes.Newobj, lazyConstructor);
        }

        /// <summary>
        /// Generates the IL required to push the value of the field to the stack, including the case where the field
        /// is lazily loaded.
        /// </summary>
        /// <param name="il">The IL generator.</param>
        /// <param name="symbolField">The field to generate the IL for.</param>
        protected void GenerateSymbolPush([NotNull] ILGenerator il, [NotNull] FieldInfo symbolField)
        {
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, symbolField);
            if (!Options.HasFlagFast(UseLazyBinding))
            {
                return;
            }

            var getMethod = typeof(Lazy<IntPtr>).GetMethod("get_Value", BindingFlags.Instance | BindingFlags.Public);
            il.Emit(OpCodes.Callvirt, getMethod);
        }

        /// <summary>
        /// Generates a lambda method for loading the given symbol.
        /// </summary>
        /// <param name="symbolName">The name of the symbol.</param>
        /// <returns>A method which, when called, will load and return the given symbol.</returns>
        [NotNull]
        protected MethodBuilder GenerateSymbolLoadingLambda([NotNull] string symbolName)
        {
            var uniqueIdentifier = Guid.NewGuid().ToString().Replace("-", "_");

            var loadSymbolMethod = typeof(AnonymousImplementationBase).GetMethod
            (
                "LoadSymbol",
                BindingFlags.NonPublic | BindingFlags.Instance
            );

            // Generate lambda loader
            var lambdaBuilder = TargetType.DefineMethod
            (
                $"{symbolName}_{uniqueIdentifier}_lazy",
                MethodAttributes.Private | MethodAttributes.HideBySig | MethodAttributes.Final,
                typeof(IntPtr),
                null
            );

            var lambdaIL = lambdaBuilder.GetILGenerator();
            lambdaIL.Emit(OpCodes.Ldarg_0);
            lambdaIL.Emit(OpCodes.Ldstr, symbolName);
            lambdaIL.EmitCall(OpCodes.Call, loadSymbolMethod, null);
            lambdaIL.Emit(OpCodes.Ret);
            return lambdaBuilder;
        }

        /// <summary>
        /// Generates a lambda method for loading the given function.
        /// </summary>
        /// <param name="delegateType">The type of delegate to load.</param>
        /// <param name="functionName">The name of the function.</param>
        /// <returns>A method which, when called, will load and return the given function.</returns>
        [NotNull]
        protected MethodBuilder GenerateFunctionLoadingLambda([NotNull] Type delegateType, [NotNull] string functionName)
        {
            var uniqueIdentifier = Guid.NewGuid().ToString().Replace("-", "_");

            var loadFuncMethod = typeof(AnonymousImplementationBase).GetMethod
            (
                "LoadFunction",
                BindingFlags.NonPublic | BindingFlags.Instance
            );

            var loadFunc = loadFuncMethod.MakeGenericMethod(delegateType);

            // Generate lambda loader
            var lambdaBuilder = TargetType.DefineMethod
            (
                $"{delegateType.Name}_{uniqueIdentifier}_lazy",
                MethodAttributes.Private | MethodAttributes.HideBySig | MethodAttributes.Final,
                delegateType,
                null
            );

            var lambdaIL = lambdaBuilder.GetILGenerator();
            lambdaIL.Emit(OpCodes.Ldarg_0);
            lambdaIL.Emit(OpCodes.Ldstr, functionName);
            lambdaIL.EmitCall(OpCodes.Call, loadFunc, null);
            lambdaIL.Emit(OpCodes.Ret);
            return lambdaBuilder;
        }
    }
}