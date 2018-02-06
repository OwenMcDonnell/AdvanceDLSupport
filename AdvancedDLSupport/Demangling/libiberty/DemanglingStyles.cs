//
//  DemanglingStyles.cs
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

// ReSharper disable UnusedMember.Global
#pragma warning disable 1591, SA1602

namespace AdvancedDLSupport.Demangling
{
    /// <summary>
    /// Demangling styles used for demangling symbols.
    /// </summary>
    public enum DemanglingStyles
    {
        None = -1,
        Unknown = 0,
        Auto = 1 << 8,
        GNU = 1 << 9,
        Lucid = 1 << 10,
        Arm = 1 << 1,
        HP = 1 << 12,
        EDG = 1 << 13,
        GNUv3 = 1 << 14,
        Java = 1 << 2,
        GNAT = 1 << 15,
        DLang = 1 << 16
    }
}
