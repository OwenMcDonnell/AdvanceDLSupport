//
//  IDemangle.cs
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
    /// Demangling functions from libiberty.
    /// </summary>
    public interface IDemangle
    {
        /// <summary>
        /// Converts a given mangled name into the mangling style used for the name.
        /// </summary>
        /// <param name="name">A mangled name.</param>
        /// <returns>The style used to mangle the name.</returns>
        DemanglingStyles NameToStyle(string name);

        /// <summary>
        /// Sets the style used for demangling.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <returns>Unknown.</returns>
        DemanglingStyles SetStyle(DemanglingStyles style);

        /// <summary>
        /// Demangle the given name.
        /// </summary>
        /// <param name="mangled">The name to demangle.</param>
        /// <param name="options">The demangling options to use.</param>
        /// <returns>The demangled name.</returns>
        string Demangle(string mangled, int options);
    }
}
