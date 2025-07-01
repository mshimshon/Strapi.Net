using Strapi.Net.Dto;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Strapi.Net.Utilities;
internal class StrapiBlockJsonConverter : JsonConverter<StrapiBlockResponse>
{
    public override StrapiBlockResponse Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        var component = root.GetProperty("__component").GetString();
        var rawJson = root.GetRawText();

        return new StrapiBlockResponse
        {
            Component = component!,
            RawContent = rawJson
        };
    }

    public override void Write(Utf8JsonWriter writer, StrapiBlockResponse value, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.Parse(value.RawContent);
        doc.RootElement.WriteTo(writer);
    }
}