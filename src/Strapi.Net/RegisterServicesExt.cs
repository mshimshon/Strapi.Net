using Microsoft.Extensions.DependencyInjection;
using Strapi.Net.Internal;

namespace Strapi.Net;
public static class RegisterServicesExt
{
    public static IServiceCollection AddStrapi(this IServiceCollection services)
    {
        services.AddTransient<IStrapiBuilder, StrapiQueryBuilder>();
        return services;
    }
}
