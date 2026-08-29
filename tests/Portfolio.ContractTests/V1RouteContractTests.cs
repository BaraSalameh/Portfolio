using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Portfolio.Controllers;
using Microsoft.AspNetCore.RateLimiting;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;

namespace Portfolio.ContractTests;

public sealed class V1RouteContractTests
{
    [Fact]
    public void SharedApiController_DeclaresLegacyAndExplicitV1Routes()
    {
        var routes = typeof(ApiController).GetCustomAttributes(true)
            .OfType<RouteAttribute>()
            .Select(route => route.Template)
            .ToArray();
        var version = Assert.Single(typeof(ApiController).GetCustomAttributes(true).OfType<ApiVersionAttribute>());

        Assert.Contains("api/[controller]/[action]", routes);
        Assert.Contains("api/v{version:apiVersion}/[controller]/[action]", routes);
        Assert.Contains(new ApiVersion(1, 0), version.Versions);
    }

    [Theory]
    [InlineData(typeof(AccountController), 6)]
    [InlineData(typeof(AdminController), 8)]
    [InlineData(typeof(ClientController), 3)]
    [InlineData(typeof(OwnerController), 44)]
    public void ExistingV1ControllerActionsRemainAttributeRouted(Type controllerType, int expectedActions)
    {
        var actions = controllerType.GetMethods()
            .Where(method => method.DeclaringType == controllerType)
            .Where(method => method.GetCustomAttributes(typeof(HttpMethodAttribute), true).Length > 0)
            .ToArray();

        Assert.Equal(expectedActions, actions.Length);
        Assert.All(actions, action => Assert.NotNull(action.GetCustomAttributes(true)
            .OfType<HttpMethodAttribute>()
            .SingleOrDefault()));
    }

    [Theory]
    [InlineData(nameof(AccountController.Login))]
    [InlineData(nameof(AccountController.Register))]
    [InlineData(nameof(AccountController.Logout))]
    [InlineData(nameof(AccountController.ValidateToken))]
    [InlineData(nameof(AccountController.ConfirmEmail))]
    [InlineData(nameof(AccountController.ResendConfirmEmail))]
    public void AccountEndpoints_AreRateLimited(string actionName)
    {
        var action = typeof(AccountController).GetMethod(actionName);

        Assert.NotNull(action);
        var rateLimit = action.GetCustomAttributes(true).OfType<EnableRateLimitingAttribute>().SingleOrDefault();
        Assert.NotNull(rateLimit);
        Assert.Equal("authentication", rateLimit.PolicyName);
    }

    [Theory]
    [InlineData(typeof(AdminController), "RequireAdminRole")]
    [InlineData(typeof(OwnerController), "RequireOwnerRole")]
    public void PrivilegedControllers_RequireTheirExplicitRolePolicy(Type controllerType, string policy)
    {
        var authorization = controllerType.GetCustomAttributes(true)
            .OfType<Microsoft.AspNetCore.Authorization.AuthorizeAttribute>()
            .SingleOrDefault();

        Assert.NotNull(authorization);
        Assert.Equal(policy, authorization.Policy);
    }

    [Theory]
    [InlineData(typeof(AccountController))]
    [InlineData(typeof(ClientController))]
    [InlineData(typeof(MaintenanceController))]
    public void IntentionalPublicControllers_ExplicitlyOptOutOfFallbackAuthorization(Type controllerType)
    {
        Assert.NotNull(controllerType.GetCustomAttributes(true)
            .OfType<AllowAnonymousAttribute>()
            .SingleOrDefault());
    }

    [Theory]
    [InlineData(nameof(ClientController.UserList))]
    [InlineData(nameof(ClientController.UserByUsername))]
    public void CacheablePublicReads_VarySharedRepresentationsByOrigin(string actionName)
    {
        var action = typeof(ClientController).GetMethod(actionName);

        Assert.NotNull(action);
        var cache = Assert.Single(action.GetCustomAttributes(true).OfType<ResponseCacheAttribute>());
        Assert.Equal(ResponseCacheLocation.Any, cache.Location);
        Assert.False(cache.NoStore);
        Assert.Equal(60, cache.Duration);
        Assert.Equal("Origin", cache.VaryByHeader);
    }
}
