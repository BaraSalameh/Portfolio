using System.Globalization;

namespace Application.Common.Text;

public static class SearchTerm
{
    public static string Normalize(string value) =>
        value.ToLower(CultureInfo.InvariantCulture);
}
