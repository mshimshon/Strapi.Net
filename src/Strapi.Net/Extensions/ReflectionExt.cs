using System.Linq.Expressions;
using System.Reflection;

namespace Strapi.Net.Extensions;
public static class ReflectionExt
{
    public static KeyValuePair<Expression<Func<TEntity, TValue>>, TValue> PropSetToKeyPair<TEntity, TValue>(Expression<Func<TEntity, TValue>> property, TValue value)
    {
        var propname = property.GetPropetyName();
        return new KeyValuePair<Expression<Func<TEntity, TValue>>, TValue>(property, value);
    }

    public static string GetPropetyName<TValue>(this Expression<Func<TValue>> property)
        => property.GetPropetyInfo().Name;

    public static string GetPropetyName<TEntity, TValue>(this Expression<Func<TEntity, TValue>> property)
    => property.GetPropetyInfo().Name;

    public static PropertyInfo GetPropetyInfo<TEntity, TValue>(this Expression<Func<TEntity, TValue>> property)
    {
        var memberExpression = property.Body is UnaryExpression expression
            ? (MemberExpression)expression.Operand
            : (MemberExpression)property.Body;
        return (PropertyInfo)memberExpression.Member;
    }

    public static PropertyInfo GetPropetyInfo<TValue>(this Expression<Func<TValue>> property)
    {
        var memberExpression = property.Body is UnaryExpression expression
            ? (MemberExpression)expression.Operand
            : (MemberExpression)property.Body;
        return (PropertyInfo)memberExpression.Member;
    }
    public static string GetFullPropertyPath<TEntity>(this Expression<Func<TEntity, object>> property)
    {
        var memberExpression = property.Body switch
        {
            MemberExpression m => m,
            UnaryExpression u when u.Operand is MemberExpression m => m,
            _ => throw new ArgumentException("Invalid expression format", nameof(property))
        };

        var stack = new Stack<string>();
        while (memberExpression != null)
        {
            stack.Push(memberExpression.Member.Name);
            memberExpression = memberExpression.Expression as MemberExpression;
        }

        return string.Join(".", stack);
    }
}