using System.Globalization;
using Application.Common.Text;

namespace Portfolio.UnitTests;

public sealed class SearchTermTests
{
    [Fact]
    public void Normalize_IsStableUnderTurkishProcessCulture()
    {
        var originalCulture = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("tr-TR");

            Assert.Equal("institution", SearchTerm.Normalize("INSTITUTION"));
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
        }
    }
}
