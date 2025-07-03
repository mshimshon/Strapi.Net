using Strapi.Net.Dto;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Strapi.Net.Utilities;
public static class StrapiDecoder
{
    public static StrapiResponse<TData> DecodeResponse<TData>(string json)
    {
        var response = new StrapiResponse<TData>();

        var root = JsonNode.Parse(json);
        var optionSerializer = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };
        optionSerializer.Converters.Add(new StrapiBlockJsonConverter());
        if (root?["data"] is JsonNode dataNode)
        {
            if (dataNode is JsonArray)
            {
                var list = dataNode.Deserialize<List<TData>>(optionSerializer);
                if (list is { Count: > 0 })
                    response.Data = list;
            }
            else if (dataNode is JsonObject || dataNode is JsonValue) // handle edge case where object is wrapped in value
            {
                var single = dataNode.Deserialize<TData>(optionSerializer);
                if (single is not null)
                    response.Data = new List<TData> { single };
            }
        }
        // Handle "error"
        if (root?["error"] is JsonNode errorNode)
        {
            var error = errorNode.Deserialize<StrapiError>();
            if (error != default)
                response.Error = error;
        }


        if (root?["meta"] is JsonNode metaNode)
            if (metaNode is JsonObject || metaNode is JsonValue) // handle edge case where object is wrapped in value
            {
                var single = metaNode.Deserialize<StrapiMeta>(optionSerializer);
                if (single is not null)
                    response.Meta = single;
            }
        return response;
    }
}
