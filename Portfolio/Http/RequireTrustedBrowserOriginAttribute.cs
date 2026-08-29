namespace Portfolio.Http;

/// <summary>
/// Marks a legacy safe-method endpoint whose behavior changes server state and therefore
/// must reject cross-site browser requests even though its HTTP method cannot be changed
/// without breaking the v1 contract.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class RequireTrustedBrowserOriginAttribute : Attribute;
