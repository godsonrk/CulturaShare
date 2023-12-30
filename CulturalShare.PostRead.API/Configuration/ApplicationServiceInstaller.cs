using CulturalShare.PostRead.API.Configuration.Base;
using CulturalShare.PostRead.Services.DependencyInjection;

namespace CulturalShare.PostRead.API.Configuration;

public class ApplicationServiceInstaller : IServiceInstaller
{
    public void Install(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddPostsReadServices();
    }
}
