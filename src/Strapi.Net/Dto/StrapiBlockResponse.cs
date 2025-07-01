namespace Strapi.Net.Dto;
public record StrapiBlockResponse
{
    public string Component { get; set; } = default!;
    public string RawContent { get; set; } = default!;
}
