namespace Portfolio.Http;

public static class ApiRoutePaths
{
    public const string LegacyAccount = "/api/Account";
    public const string V1Account = "/api/v1/Account";

    public static bool IsController(PathString path, string controller)
    {
        var segments = path.Value?.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments is not { Length: >= 2 } ||
            !string.Equals(segments[0], "api", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        var controllerIndex = IsVersionSegment(segments[1]) ? 2 : 1;
        return segments.Length > controllerIndex &&
            string.Equals(segments[controllerIndex], controller, StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsVersionSegment(string segment) =>
        segment.Length > 1 &&
        (segment[0] == 'v' || segment[0] == 'V') &&
        char.IsDigit(segment[1]);
}
