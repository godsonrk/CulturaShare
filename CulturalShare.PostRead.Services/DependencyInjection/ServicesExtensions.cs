using CulturalShare.PostRead.Services.Services;
using CulturalShare.PostRead.Services.Services.Base;
using Microsoft.Extensions.DependencyInjection;

namespace CulturalShare.PostRead.Services.DependencyInjection;

public static class ServicesExtension
{
    public static IServiceCollection AddPostsReadServices(this IServiceCollection services)
    {
        services.AddScoped<IPostService, PostService>();

        return services;
    }
}
