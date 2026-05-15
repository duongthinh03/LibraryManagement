using System.Linq.Expressions;
using Library.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Common
{
    public static class QueryableExtensions
    {
        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
            this IQueryable<T> query,
            int pageNo,
            int pageSize)
        {
            var totalItems = await query.CountAsync();

            var data = await query
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<T>
            {
                Data = data,
                TotalItems = totalItems
            };
        }

        public static IQueryable<T> ApplyFilter<T>(
            this IQueryable<T> query,
            Dictionary<string, object> filters)
        {
            foreach (var filter in filters)
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                Expression property = parameter;

                foreach (var member in filter.Key.Split('.'))
                {
                    property = Expression.PropertyOrField(property, member);
                }

                var targetType = Nullable.GetUnderlyingType(property.Type) ?? property.Type;
                var filterValue = Convert.ChangeType(filter.Value, targetType);
                var constant = Expression.Constant(filterValue, targetType);
                Expression valueExpression = property.Type == targetType
                    ? constant
                    : Expression.Convert(constant, property.Type);

                var body = Expression.Equal(property, valueExpression);
                var predicate = Expression.Lambda<Func<T, bool>>(body, parameter);

                query = query.Where(predicate);
            }

            return query;
        }

        public static IQueryable<T> ApplySorting<T>(
            this IQueryable<T> query,
            string sortBy,
            string? sortDirection)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            Expression property = parameter;

            foreach (var member in sortBy.Split('.'))
            {
                property = Expression.PropertyOrField(property, member);
            }

            var keySelector = Expression.Lambda(property, parameter);
            var methodName = string.Equals(sortDirection, "DESC", StringComparison.OrdinalIgnoreCase)
                ? nameof(Queryable.OrderByDescending)
                : nameof(Queryable.OrderBy);

            var result = typeof(Queryable).GetMethods()
                .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), property.Type)
                .Invoke(null, new object[] { query, keySelector });

            return (IQueryable<T>)result!;
        }
    }
}
