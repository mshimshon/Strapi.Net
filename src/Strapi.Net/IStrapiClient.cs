using Strapi.Net.Dto;

namespace Strapi.Net;
public interface IStrapiClient
{
    Task<StrapiResponse<TEntity>?> GetAsync<TEntity>(string uri, CancellationToken cancellationToken = default);
    Task<StrapiResponse<TEntity>?> PostAsync<TEntity>(string uri, HttpContent httpContent, CancellationToken cancellationToken = default);
    Task<StrapiResponse<TEntity>?> PatchAsync<TEntity>(string uri, HttpContent httpContent, CancellationToken cancellationToken = default);
    Task<StrapiResponse<TEntity>?> PutAsync<TEntity>(string uri, HttpContent httpContent, CancellationToken cancellationToken = default);
    Task<StrapiResponse<TEntity>?> DeleteAsync<TEntity>(string uri, CancellationToken cancellationToken = default);

}
