namespace Strapi.Net.Dto;
public record StrapiErrorField
{
    public string[]? Path { get; set; }
    public string? Message { get; set; }
}
