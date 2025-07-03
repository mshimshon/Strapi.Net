using Strapi.Net.Enums;
using Strapi.Net.Extensions;
using Strapi.Net.Internal;
using System.Linq.Expressions;

namespace Strapi.Net;
public class StrapiQueryBuilder : IStrapiBuilder
{
    private readonly Dictionary<string, string> _filters = new();
    private readonly HashSet<string> _populate = new();
    private readonly HashSet<string> _sorts = new();
    private readonly HashSet<string> _fields = new();

    private string _search = string.Empty;
    private string _locale = string.Empty;
    private bool _isPopulatingAll = false;
    private bool _isDraftOnly = false;
    private int? _page = null;
    private int? _pageSize = null;
    public StrapiQueryBuilder()
    {
    }
    public static IStrapiBuilder Create() => new StrapiQueryBuilder();
    public IStrapiBuilder AddSort(string field, StrapiShortDirection direction = StrapiShortDirection.Ascending)
    {
        _sorts.Add($"{field}:{StrapiEnumMappings._sortDirection[direction]}");
        return this;
    }

    public IStrapiBuilder AddSort<TEntity>(Expression<Func<TEntity, object>> property, StrapiShortDirection direction = StrapiShortDirection.Ascending)
      => AddSort(property.GetFullPropertyPath(), direction);

    public IStrapiBuilder Filter(string field, StrapiFilterOperator @operator, string value)
    {
        _filters.Add($"[{field}]" + $"[{StrapiEnumMappings._operatorMap[@operator]}]", value);
        return this;
    }
    public IStrapiBuilder Filter(string[] fields, StrapiFilterOperator @operator, string value)
    {

        _filters.Add(fields.Select(p => $"[{p}]") + $"[{StrapiEnumMappings._operatorMap[@operator]}]", value);
        return this;
    }
    public IStrapiBuilder Filter<TEntity>(Expression<Func<TEntity, object>> property, StrapiFilterOperator @operator, string value)
    {
        var name = property.GetFullPropertyPath();
        return Filter(name, @operator, value);
    }

    public IStrapiBuilder Paginate(int page, int pageSize)
    {
        _page = page;
        _pageSize = pageSize;
        return this;
    }
    public IStrapiBuilder Populate(params string[] fields)
    {
        foreach (var item in fields)
            _populate.Add(item);
        return this;
    }

    public IStrapiBuilder RestrictFieldsTo(params string[] fields)
    {
        foreach (var item in fields)
            _fields.Add(item);
        return this;
    }
    public IStrapiBuilder RestrictFieldTo<TEntity>(Expression<Func<TEntity, object>> property, StrapiFilterOperator @operator,
        string value) => RestrictFieldsTo(property.GetFullPropertyPath());
    public IStrapiBuilder Search(string keywords) => OneLine(() => _search = keywords);
    public IStrapiBuilder SetLocal(string language) => OneLine(() => _locale = language);
    public IStrapiBuilder PopulateAll() => OneLine(() => _isPopulatingAll = true);

    public string ToQueryString(string url)
    {
        var query = new List<string>();
        if (_isDraftOnly) query.Add($"status=draft");
        if (_locale != string.Empty) query.Add($"local={_locale}");
        if (_search != string.Empty) query.Add($"_q={_search}");
        if (_filters.Count > 0)
            foreach (var f in _filters)
                query.Add($"filters{f.Key.ToLower()}={f.Value.ToString() ?? string.Empty}");

        if (!_isPopulatingAll && _populate.Count > 0)
        {
            var counter = 0;
            foreach (var item in query)
            {
                query.Add($"populate[{counter}]={item}");
                counter++;
            }
        }
        else if (_isPopulatingAll)
            query.Add("populate=*");


        if (_page.HasValue && _pageSize.HasValue)
        {
            query.Add($"pagination[page]={_page}");
            query.Add($"pagination[pageSize]={_pageSize}");
        }

        if (_sorts.Count > 0)
            query.Add($"sort={string.Join(',', _sorts)}");

        return url + "?" + string.Join("&", query);
    }
    private IStrapiBuilder OneLine(Action setter)
    {
        setter();
        return this;
    }

    public IStrapiBuilder DraftOnly() => OneLine(() => _isDraftOnly = true);
}

