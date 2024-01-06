using AuthenticationProto;
using CulturalShare.Gateway.Configuration.Base;
using CulturalShare.Gateway.Configuration.Model;
using PostsReadProto;
using PostsWriteProto;

namespace CulturalShare.Gateway.Configuration;

public class GrpcClientServiceInstaller : IServiceInstaller
{
    public void Install(WebApplicationBuilder builder)
    {
        var urls = builder.Configuration
            .GetSection("GrpcClientsUrls")
            .Get<GrpcClientsUrlModel>();

        builder.Services.AddGrpcClient<Authentication.AuthenticationClient>(options =>
        {
            options.Address = new Uri(urls.AuthClient);
        });

        builder.Services.AddGrpcClient<PostsRead.PostsReadClient>(options =>
        {
            options.Address = new Uri(urls.PostReadClient);
        });

        builder.Services.AddGrpcClient<PostsWrite.PostsWriteClient>(options =>
        {
            options.Address = new Uri(urls.PostWriteClient);
        });
    }
}
