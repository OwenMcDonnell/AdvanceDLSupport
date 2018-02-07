﻿//
//  TypeExtensions.cs
//
//  Copyright (c) 2018 Firwood Software
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using AdvancedDLSupport.Reflection;
using JetBrains.Annotations;

namespace AdvancedDLSupport.Extensions
{
    /// <summary>
    /// Extension methods for the <see cref="Type"/> class.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Gets the methods defined in the given type as wrapped introspective methods.
        /// </summary>
        /// <param name="this">The type to inspect.</param>
        /// <returns>The methods.</returns>
        public static IEnumerable<IntrospectiveMethodInfo> GetIntrospectiveMethods(this Type @this)
        {
            var methods = @this.GetMethods();
            foreach (var method in methods)
            {
                yield return new IntrospectiveMethodInfo(method);
            }
        }

        /// <summary>
        /// Gets a method defined in the given type by its name and parameter types.
        /// </summary>
        /// <param name="this">The type to inspect.</param>
        /// <param name="name">The name of the method.</param>
        /// <param name="parameterTypes">The parameter types of the method.</param>
        /// <returns>The method.</returns>
        public static IntrospectiveMethodInfo GetIntrospectiveMethod(this Type @this, string name, Type[] parameterTypes)
        {
            var method = @this.GetMethod(name, parameterTypes);
            return method is null ? null : new IntrospectiveMethodInfo(method);
        }

        /// <summary>
        /// Determines whether or not the given type requires lowering to a more primitive type when being marshalled.
        /// </summary>
        /// <param name="this">The type.</param>
        /// <returns>true if the type requires lowering; otherwise, false.</returns>
        [PublicAPI, Pure]
        public static bool RequiresLowering([NotNull] this Type @this)
        {
            return
                @this == typeof(string) ||
                @this == typeof(bool) ||
                @this.IsNonRefNullable();
        }

        /// <summary>
        /// Determines whether or not the given type is a <see cref="Nullable{T}"/> that is not passed by reference.
        /// </summary>
        /// <param name="this">The type.</param>
        /// <returns>true if it is a nullable that is not passed by reference; otherwise, false.</returns>
        public static bool IsNonRefNullable(this Type @this)
        {
            if (@this.IsByRef)
            {
                return false;
            }

            return @this.IsGenericType &&
                   @this.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// Determines whether or not the given type is a <see cref="Nullable{T}"/> passed by reference.
        /// </summary>
        /// <param name="this">The type.</param>
        /// <returns>true if it is a nullable passed by reference; otherwise, false.</returns>
        public static bool IsRefNullable(this Type @this)
        {
            if (!@this.IsByRef)
            {
                return false;
            }

            var underlying = @this.GetElementType();

            return underlying.IsGenericType &&
                   underlying.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}
