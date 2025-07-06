using Strapi.Net.Enums;
using System.Linq.Expressions;

namespace Strapi.Net;
/// <summary>
/// Strapi v5 Builder
/// </summary>
public interface IStrapiBuilder
{
    IStrapiBuilder SetLocal(string language);
    IStrapiBuilder Search(string keywords);

    IStrapiBuilder DraftOnly();
    IStrapiBuilder RestrictFieldsTo(params string[] fields);
    IStrapiBuilder RestrictFieldTo<TEntity>(Expression<Func<TEntity, object>> property, Func<string, string> caseFixer, StrapiFilterOperator @operator, string value);
    IStrapiBuilder Filter(string[] fields, StrapiFilterOperator @operator, string value);
    IStrapiBuilder Filter<TEntity>(Expression<Func<TEntity, object>> property, Func<string, string> caseFixer, StrapiFilterOperator @operator, string value);
    IStrapiBuilder Populate(params string[] fields);
    IStrapiBuilder PopulateAll();
    IStrapiBuilder AddSort(string field, StrapiShortDirection direction = StrapiShortDirection.Ascending);
    IStrapiBuilder AddSort<TEntity>(Expression<Func<TEntity, object>> property, Func<string, string> caseFixer, StrapiShortDirection direction = StrapiShortDirection.Ascending);
    IStrapiBuilder Paginate(int page, int pageSize);
    public string ToQueryString(string url);

}

