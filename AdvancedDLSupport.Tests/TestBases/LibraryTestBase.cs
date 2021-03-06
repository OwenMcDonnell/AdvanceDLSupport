﻿using System.Diagnostics.CodeAnalysis;

namespace AdvancedDLSupport.Tests.TestBases
{
    public abstract class LibraryTestBase<T> where T : class
    {
        protected readonly ImplementationOptions Config;
        protected readonly T Library;

        [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
        public LibraryTestBase(string libraryLocation)
        {
            Config = GetImplementationOptions();
            Library = GetImplementationBuilder().ActivateInterface<T>(libraryLocation);
        }

        protected virtual ImplementationOptions GetImplementationOptions()
        {
            return 0;
        }

        protected virtual NativeLibraryBuilder GetImplementationBuilder()
        {
            return new NativeLibraryBuilder(Config);
        }
    }
}