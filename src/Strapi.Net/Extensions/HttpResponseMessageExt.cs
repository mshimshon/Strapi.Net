using Strapi.Net.Dto;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Strapi.Net.Extensions;
internal static class HttpResponseMessageExt
{
    public static async Task<StrapiResponse<TData>> FromStrapiResponse<TData>(
        this HttpResponseMessage message,
        CancellationToken cancellationToken = default)
    {
        var rawJsonResponse = await message.Content.ReadAsStringAsync(cancellationToken);
        var jsonNode = JsonNode.Parse(rawJsonResponse)!;

        var isArray = false;
        var isDataNull = false;

        if (jsonNode["data"] is JsonArray)
            isArray = true;
        else if (jsonNode["data"] == null)
            isDataNull = true;

        StrapiResponse<TData> deserializedData;

        if (isDataNull || isArray)
            deserializedData = JsonSerializer.Deserialize<StrapiResponse<TData>>(rawJsonResponse)!;
        else
        {
            var dataNode = jsonNode["data"]!;
            jsonNode.AsObject().Remove("data");
            deserializedData = JsonSerializer.Deserialize<StrapiResponse<TData>>(jsonNode.ToJsonString())!;
            var singleItem = JsonSerializer.Deserialize<TData>(dataNode.ToJsonString())!;
            deserializedData.Data = new List<TData>() { singleItem };
        }
        return deserializedData;
    }

}
