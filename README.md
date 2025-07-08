# Strapi.Net

**Strapi.Net** is a lightweight helper library for .NET applications integrating with Strapi-powered backends. It provides:

* ✅ Fluent Strapi query building with `IStrapiBuilder`
* ✅ Seamless deserialization of Strapi single/collection responses
* ✅ Error model mapping from Strapi's standard format
* ✅ Fully testable and abstract-friendly design

---

## 🚀 Features

### 🛠 Fluent Query Builder

Build complex Strapi query strings without manually encoding filters, sorts, or relations:

```csharp
var query = strapiBuilder
    .From("articles")
    .Filter("title", StrapiFilterOperator.Equal, "AI")
    .Sort("publishedAt", StrapiSortDirection.Descending)
    .Paginate(1, 10)
    .Populate("author", "tags")
    .Build();
```

### 🔄 Response Mapping

Map HTTP responses directly into strongly typed Strapi models:

```csharp
HttpResponseMessage response = await httpClient.GetAsync(query);
var result = await response.FromStrapi<ArticleDto>();

if (!result.IsError)
{
    var articles = result.Content;
    var meta = result.StrapiRawResponse.Meta;
}
```

### ⚠️ Error Handling

Strapi-formatted error responses are mapped into:

```csharp
public record StrapiError
{
    public int Status { get; set; }
    public string Name { get; set; } = default!;
    public string Message { get; set; } = default!;
    public StrapiErrorDetails? Details { get; set; }
}
```

---

## 💡 Ideal Use Case

* ASP.NET Core / Blazor apps using Strapi
* Clean Architecture or CQRS-based systems
* Frontends where HttpClient integration needs to remain clean and abstract

---

## 🧪 Sample Models

```csharp
public record StrapiResponse<T>
{
    public List<T> Data { get; set; } = new();
    public StrapiMeta Meta { get; set; } = default!;
    public List<StrapiError>? Errors { get; set; }
}

public record StrapiMeta
{
    public StrapiPagination Pagination { get; set; } = default!;
}

public record StrapiPagination
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int PageCount { get; set; }
    public int Total { get; set; }
}
```


## 🧩 Installation

```bash
# (soon to be published)
dotnet add package Strapi.Net
```

## 🧩 Implement ```IStrapiClient```
You must implement the client yourself to ensure loose coupleling with httpclient.

Here's an example:

```bash
internal class StrapiClient : IStrapiClient
{
    private readonly HttpClient _client;
    private readonly IResourceProvider<ApplicationResource> _appResourceProvider;
    private readonly IDispatcher _dispatcher;

    public StrapiClient(HttpClient client,
        IResourceProvider<ApplicationResource> AppResourceProvider,
        IDispatcher dispatcher)
    {
        _client = client;
        _appResourceProvider = AppResourceProvider;
        _dispatcher = dispatcher;
#if DEBUG
        _client.BaseAddress = new("http://localhost:1337/api/");
#else
        _client.BaseAddress = new("https://api.bneimikra.com/api/");
#endif
    }
    private async Task<TResult?> RequestHandler<TResult>(Func<Task<TResult>> call)
    {
        try
        {
            return await call();
        }
        catch (Exception ex)
        {
            await _dispatcher.Prepare<PushErrorMessageAction>()
                .With(p => p.Message, _appResourceProvider.GetString(() => ApplicationResource.HttpStatusCodeUnknown))
                .DispatchAsync();
        }
        return default;
    }
    public async Task<StrapiResponse<TEntity>?> DeleteAsync<TEntity>(string uri, CancellationToken cancellationToken = default)
    {

        var result = await RequestHandler(() => _client.DeleteAsync(uri, cancellationToken));
        if (result == default) return default;
        if (!result.IsSuccessStatusCode) await HandleErrors(result);
        if (result != default && result.Content != default)
        {
            var json = await result.Content.ReadAsStringAsync();
            return StrapiDecoder.DecodeResponse<TEntity>(json);
        }
        return default;
    }

    public async Task<StrapiResponse<TEntity>?> GetAsync<TEntity>(string uri, CancellationToken cancellationToken = default)
    {

        var result = await RequestHandler(() => _client.GetAsync(uri, cancellationToken));
        if (result == default) return default;

        if (!result.IsSuccessStatusCode) await HandleErrors(result);
        if (result != default && result.Content != default)
        {
            var json = await result.Content.ReadAsStringAsync();
            return StrapiDecoder.DecodeResponse<TEntity>(json);
        }
        return default;
    }

    public async Task<StrapiResponse<TEntity>?> PatchAsync<TEntity>(string uri, HttpContent httpContent, CancellationToken cancellationToken = default)
    {
        var result = await RequestHandler(() => _client.PatchAsync(uri, httpContent, cancellationToken));
        if (result == default) return default;
        if (!result.IsSuccessStatusCode) await HandleErrors(result);



        if (result != default && result.Content != default)
        {
            var json = await result.Content.ReadAsStringAsync();
            return StrapiDecoder.DecodeResponse<TEntity>(json);
        }
        return default;
    }
    public async Task<StrapiResponse<TEntity>?> PostAsync<TEntity>(string uri, HttpContent httpContent, CancellationToken cancellationToken = default)
    {
        var result = await RequestHandler(() => _client.PostAsync(uri, httpContent, cancellationToken));
        if (result == default) return default;
        if (!result.IsSuccessStatusCode) await HandleErrors(result);
        if (result != default && result.Content != default)
        {
            var json = await result.Content.ReadAsStringAsync();
            return StrapiDecoder.DecodeResponse<TEntity>(json);
        }
        return default;
    }
    public async Task<StrapiResponse<TEntity>?> PutAsync<TEntity>(string uri, HttpContent httpContent, CancellationToken cancellationToken = default)
    {
        var result = await RequestHandler(() => _client.PutAsync(uri, httpContent, cancellationToken));
        if (result == default) return default;
        if (!result.IsSuccessStatusCode) await HandleErrors(result);
        if (result != default && result.Content != default)
        {
            var json = await result.Content.ReadAsStringAsync();
            return StrapiDecoder.DecodeResponse<TEntity>(json);
        }
        return default;
    }

    private async Task HandleErrors(HttpResponseMessage message)
    {

        if (message.StatusCode == System.Net.HttpStatusCode.NotFound)
            await _dispatcher.Prepare<PushErrorMessageAction>()
                .With(p => p.Message, _appResourceProvider.GetString(() => ApplicationResource.HttpStatusNotFound))
                .DispatchAsync();
        else if (message.StatusCode == System.Net.HttpStatusCode.BadRequest)
            await _dispatcher.Prepare<PushErrorMessageAction>()
                .With(p => p.Message, _appResourceProvider.GetString(() => ApplicationResource.HttpStatusCodeBadRequest))
                .DispatchAsync();
        else if (message.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            await _dispatcher.Prepare<PushErrorMessageAction>()
                .With(p => p.Message, _appResourceProvider.GetString(() => ApplicationResource.HttpStatusCodeUnauthorized))
                .DispatchAsync();
        else if (message.StatusCode == System.Net.HttpStatusCode.Forbidden)
            await _dispatcher.Prepare<PushErrorMessageAction>()
                .With(p => p.Message, _appResourceProvider.GetString(() => ApplicationResource.HttpStatusCodeForbidden))
                .DispatchAsync();
        else
            await _dispatcher.Prepare<PushErrorMessageAction>()
                .With(p => p.Message, _appResourceProvider.GetString(() => ApplicationResource.HttpStatusCodeUnknown))
                .DispatchAsync();
    }
}

```


## 📄 License

MIT will always stay Free!

MIT © Maksim Shimshon
