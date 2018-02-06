using AdvancedDLSupport.Demangling;
using Xunit;

namespace AdvancedDLSupport.Tests.Integration.Demangling
{
    public class SymbolManglerTests
    {
        private readonly SymbolMangler _mangler;

        public SymbolManglerTests()
        {
            _mangler = new SymbolMangler();
        }

        [Fact]
        public void CanDemangleSymbol()
        {
            const string gnuStyleSymbol = "_ZN7MyClass15MyClassFunctionEii";
            const string expected = "MyClass::MyClassFunction(int, int)";

            var actual = _mangler.Demangle(gnuStyleSymbol, DemanglingOptions.IncludeFunctionParameters, DemanglingStyles.Auto);
            Assert.Equal(expected, actual);
        }
    }
}
