using Api.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("/[controller]")]
[ExceptionFilter]
public class MediatorControllerBase : ControllerBase
{
    private readonly IMediator _mediator;

    public MediatorControllerBase(IMediator mediator)
    {
        _mediator = mediator;
    }

    protected async Task<IActionResult> Send<TResponse>(IRequest<TResponse> request, CancellationToken token) =>
        Ok(await _mediator.Send(request, token));
}