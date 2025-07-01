namespace Strapi.Net.Dto;
public record StrapiResponse<TData>
{
    public List<TData> Data { get; set; } = new();

    public StrapiMeta? Meta { get; set; }
    public StrapiError? Error { get; set; }
}
