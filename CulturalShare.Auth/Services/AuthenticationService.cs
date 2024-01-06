using AuthenticationProto;
using Grpc.Core;

namespace CulturalShare.Auth.Services;

public class AuthenticationService : Authentication.AuthenticationBase
{
    public override Task<AccessTokenReply> Login(LoginRequest request, ServerCallContext context)
    {
        throw new RpcException(new Status(StatusCode.Unavailable, "Not implemented"));
    }

    public override Task<AccessTokenReply> GetOneTimeToken(GetOneTimeTokenRequest request, ServerCallContext context)
    {
        throw new RpcException(new Status(StatusCode.Unavailable, "Not implemented"));
    }

    public override Task<AccessTokenReply> RefreshToken(RefreshTokenRequest request, ServerCallContext context)
    {
        throw new RpcException(new Status(StatusCode.Unavailable, "Not implemented"));
    }

    public override Task<RegistrationReply> Registration(RegistrationRequest request, ServerCallContext context)
    {
        throw new RpcException(new Status(StatusCode.Unavailable, "Not implemented"));
    }
}
