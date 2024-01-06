using AuthenticationProto;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;

namespace CulturalShare.Gateway.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly Authentication.AuthenticationClient _authClient;

    public AuthController(Authentication.AuthenticationClient authClient)
    {
        _authClient = authClient;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request,CancellationToken cancellationToken)
    {
        try
        {
            var result = await _authClient.LoginAsync(request, cancellationToken: cancellationToken);
            return Ok(result);
        }
        catch (RpcException)
        {
            throw;
        }
    }

    [HttpPost("Registration")]
    public async Task<IActionResult> RegistrationAsync([FromBody] RegistrationRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _authClient.RegistrationAsync(request, cancellationToken: cancellationToken);
            return Ok(result);
        }
        catch (RpcException)
        {
            throw;
        }
    }

    [HttpPost("RefreshToken")]
    public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _authClient.RefreshTokenAsync(request, cancellationToken: cancellationToken);
            return Ok(result);
        }
        catch (RpcException)
        {
            throw;
        }
    }
}
