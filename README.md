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

---

## 🧩 Installation

```bash
# (soon to be published)
dotnet add package Strapi.Net
```

---

## 📄 License

MIT © Maksim Shimshon
