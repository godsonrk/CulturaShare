using CulturalShare.Gateway.Middleware.MiddlewareClasses;

namespace CulturalShare.Gateway.Middleware.Extension;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionsHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<HandlingExceptionsMiddleware>();
    }
}
