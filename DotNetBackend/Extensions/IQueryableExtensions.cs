
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace DotNetBackend.Extensions;

public static class QueryableExtensions
{
    // If hasKeyword == false, this is exactly the same as:
    //    _context.Posts.Include(p => p.User)
    // 
    // If hasKeyword == true, EF generates SQL like:
    //    SELECT … FROM Posts
    //    LEFT JOIN Users …
    //   WHERE Title LIKE @kw OR Description LIKE @kw
    public static IQueryable<T> WhereIf<T>(
        this IQueryable<T> source,
        bool condition,
        Expression<Func<T, bool>> predicate)
    {
        return condition
            ? source.Where(predicate)
            : source;
    }

    public static async Task<PaginatedList<T>> ApplyPaginationAsync<T>(
            this IQueryable<T> source,
            int pageSize,
            int pageIndex)
    {
        // if pageSize or pageIndex are invalid, return everything
        if (pageSize <= 0 || pageIndex <= 0)
        {
            var all = await source.ToListAsync();
            return new PaginatedList<T>
            {
                Count = all.Count,
                PageIndex = 1,
                Data = all
            };
        }

        // total count
        var count = await source.CountAsync();

        // this page’s items
        var items = await source
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedList<T>
        {
            Count = count,
            PageIndex = pageIndex,
            Data = items
        };
    }
}
