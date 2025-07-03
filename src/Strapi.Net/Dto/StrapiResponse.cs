namespace Strapi.Net.Dto;
public record StrapiResponse<TData>
{
    public ICollection<TData> Data { get; set; } = new List<TData>();

    public StrapiMeta? Meta { get; set; }
    public StrapiError? Error { get; set; }
}
