using System;

// ReSharper disable UnusedMember.Global

namespace AdvancedDLSupport.Tests.Data
{
    public interface ITypeLoweringLibrary
    {
        string GetString();
        string GetNullString();

        bool CheckIfStringIsNull(string value);
        UIntPtr StringLength(string value);
    }
}
