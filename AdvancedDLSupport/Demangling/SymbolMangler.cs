//
//  SymbolMangler.cs
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

namespace AdvancedDLSupport.Demangling
{
    /// <summary>
    /// Mangler and demangler for C++ symbols.
    /// </summary>
    public sealed class SymbolMangler
    {
        private const string LibraryName = "iberty";

        private readonly IDemangle _library;

        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolMangler"/> class.
        /// </summary>
        public SymbolMangler()
        {
            _library = new AnonymousImplementationBuilder().ResolveAndActivateInterface<IDemangle>(LibraryName);
        }

        /// <summary>
        /// Demangles the given symbol, using the specified options.
        /// </summary>
        /// <param name="mangledSymbol">The mangled symbol.</param>
        /// <param name="options">Options to use when demangling.</param>
        /// <param name="style">The style to use when demangling.</param>
        /// <returns>The demangled name.</returns>
        public string Demangle(string mangledSymbol, DemanglingOptions options, DemanglingStyles style)
        {
            var combinedOptions = (int)options | (int)style;
            return _library.Demangle(mangledSymbol, combinedOptions);
        }
    }
}
