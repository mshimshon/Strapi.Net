using Microsoft.Extensions.DependencyInjection;

namespace Strapi.Net;
public static class RegisterServicesExt
{
    public static IServiceCollection AddStrapi(this IServiceCollection services)
    {
        return services;
    }
}
