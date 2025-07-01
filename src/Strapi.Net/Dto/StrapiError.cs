namespace Strapi.Net.Dto;

public record StrapiError
{
    public int Status { get; set; }

    public string Name { get; set; } = default!;

    public string Message { get; set; } = default!;

    public StrapiErrorDetails? Details { get; set; }
}