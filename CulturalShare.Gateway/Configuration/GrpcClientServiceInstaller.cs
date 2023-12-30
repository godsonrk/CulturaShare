using CulturalShare.Gateway.Configuration.Base;
using PostsReadProto;
using PostsWriteProto;

namespace CulturalShare.Gateway.Configuration;

public class GrpcClientServiceInstaller : IServiceInstaller
{
    public void Install(WebApplicationBuilder builder)
    {
        builder.Services.AddGrpcClient<Authentication.Authentication.AuthenticationClient>(options =>
        {
            options.Address = new Uri("https://localhost:7140");
        });

        builder.Services.AddGrpcClient<PostsRead.PostsReadClient>(options =>
        {
            options.Address = new Uri("https://localhost:7102");
        });

        builder.Services.AddGrpcClient<PostsWrite.PostsWriteClient>(options =>
        {
            options.Address = new Uri("https://localhost:7143");
        });
    }
}
