﻿//
//  LibraryLoadingException.cs
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
using JetBrains.Annotations;

namespace AdvancedDLSupport
{
    /// <summary>
    /// Represents a failure to load a native library.
    /// </summary>
    [PublicAPI]
    public class LibraryLoadingException : Exception
    {
        /// <summary>
        /// Gets the name of the library that failed to load.
        /// </summary>
        [PublicAPI]
        public string LibraryName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryLoadingException"/> class.
        /// </summary>
        /// <param name="message">The message of the exception.</param>
        [PublicAPI]
        public LibraryLoadingException([CanBeNull] string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryLoadingException"/> class.
        /// </summary>
        /// <param name="message">The message of the exception.</param>
        /// <param name="libraryName">The name of the library that failed to load.</param>
        [PublicAPI]
        public LibraryLoadingException([NotNull] string message, [CanBeNull] string libraryName)
            : base(message)
        {
            LibraryName = libraryName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryLoadingException"/> class.
        /// </summary>
        /// <param name="message">The message of the exception.</param>
        /// <param name="inner">The exception which caused this exception.</param>
        /// <param name="libraryName">The name of the library that failed to load.</param>
        [PublicAPI]
        public LibraryLoadingException([NotNull] string message, [NotNull] Exception inner, [CanBeNull] string libraryName)
            : base(message, inner)
        {
            LibraryName = libraryName;
        }
    }
}
