using Strapi.Net.Enums;

namespace Strapi.Net.Internal;
internal static class StrapiEnumMappings
{
    internal static readonly IReadOnlyDictionary<StrapiFilterOperator, string> _operatorMap = new Dictionary<StrapiFilterOperator, string>
    {
        [StrapiFilterOperator.Equal] = "$eq",
        [StrapiFilterOperator.NotEqual] = "$ne",
        [StrapiFilterOperator.LessThan] = "$lt",
        [StrapiFilterOperator.LessThanOrEqual] = "$lte",
        [StrapiFilterOperator.GreaterThan] = "$gt",
        [StrapiFilterOperator.GreaterThanOrEqual] = "$gte",
        [StrapiFilterOperator.In] = "$in",
        [StrapiFilterOperator.NotIn] = "$nin",
        [StrapiFilterOperator.Contains] = "$contains",
        [StrapiFilterOperator.ContainsInsensitive] = "$containsi",
        [StrapiFilterOperator.IsNull] = "$null",
        [StrapiFilterOperator.Between] = "$between",
        [StrapiFilterOperator.StartsWith] = "$startsWith",
        [StrapiFilterOperator.EndsWith] = "$endsWith",
        [StrapiFilterOperator.And] = "$and",
        [StrapiFilterOperator.Or] = "$or",
        [StrapiFilterOperator.Not] = "$not"
    };

    internal static readonly IReadOnlyDictionary<StrapiShortDirection, string> _sortDirection = new Dictionary<StrapiShortDirection, string>
    {
        [StrapiShortDirection.Descesding] = "desc",
        [StrapiShortDirection.Ascending] = "asc",
    };

}

