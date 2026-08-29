using Application.Common.Functions;
using System.Globalization;

namespace Portfolio.UnitTests;

public sealed class AppFunctionsTests
{
    [Theory]
    [InlineData("hello WORLD", "helloWorld")]
    [InlineData("__hello___WORLD__", "helloWorld")]
    [InlineData("  hello\tWORLD  ", "helloWorld")]
    [InlineData("___", "")]
    [InlineData("", "")]
    public void ToCamelCase_IsTotalAndCanonical(string input, string expected)
    {
        Assert.Equal(expected, input.ToCamelCase());
    }

    [Theory]
    [InlineData("hello WORLD", "HelloWorld")]
    [InlineData("__hello___WORLD__", "HelloWorld")]
    [InlineData("___", "")]
    [InlineData("", "")]
    public void ToPascalCase_IsTotalAndCanonical(string input, string expected)
    {
        Assert.Equal(expected, input.ToPascalCase());
    }

    [Fact]
    public void Casing_IsInvariantUnderTurkishProcessCulture()
    {
        var originalCulture = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("tr-TR");

            Assert.Equal("identifierInput", "IDENTIFIER INPUT".ToCamelCase());
            Assert.Equal("IdentifierInput", "IDENTIFIER INPUT".ToPascalCase());
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
        }
    }
}
