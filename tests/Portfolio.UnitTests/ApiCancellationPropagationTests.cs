using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Portfolio.Controllers;

namespace Portfolio.UnitTests;

public sealed class ApiCancellationPropagationTests
{
    [Fact]
    public async Task ApiController_PropagatesRequestAbortedToMediatRHandler()
    {
        var services = new ServiceCollection();
        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssemblyContaining<CancellationRequest>());
        await using var provider = services.BuildServiceProvider();
        var controller = new TestController(provider.GetRequiredService<IMediator>());
        using var cancellation = new CancellationTokenSource();
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                RequestAborted = cancellation.Token
            }
        };

        var observedToken = await controller.Execute(new CancellationRequest());

        Assert.Equal(cancellation.Token, observedToken);
    }

    private sealed class TestController(IMediator mediator) : ApiController(mediator)
    {
        public Task<CancellationToken> Execute(CancellationRequest request) => Send(request);
    }

    public sealed record CancellationRequest : IRequest<CancellationToken>;

    public sealed class CancellationHandler : IRequestHandler<CancellationRequest, CancellationToken>
    {
        public Task<CancellationToken> Handle(
            CancellationRequest request,
            CancellationToken cancellationToken) => Task.FromResult(cancellationToken);
    }
}
