//
//  DemanglingOptions.cs
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

using System;

namespace AdvancedDLSupport.Demangling
{
    /// <summary>
    /// Demangling options. Can be combined with <see cref="DemanglingStyles"/>.
    /// </summary>
    [Flags]
    public enum DemanglingOptions
    {
        None = 0,
        IncludeFunctionParameters = 1 << 0,
        IncludeANSI = 1 << 1,
        Verbose = 1 << 3,
        IncludeTypes = 1 << 4,
        PostfixReturnTypes = 1 << 5,
        DropReturnTypes = 1 << 6
    }
}
