namespace Strapi.Net.Dto;
public record StrapiErrorDetails
{
    public List<StrapiErrorField>? Errors { get; set; }
}
