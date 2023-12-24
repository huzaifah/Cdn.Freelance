using Cdn.Freelance.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace Cdn.Freelance.Infrastructure.Repositories
{
    /// <summary>
    /// The queryable extensions dedicated to limit offset paging.
    /// </summary>
    internal static class LimitOffsetQueryableExtensions
    {
        /// <summary>
        /// Paginates the query with the specified paging parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="pagingParameters">The paging parameters.</param>
        /// <returns>
        /// The page result.
        /// </returns>
        public static async Task<LimitOffsetPagingResult<T>> Paginate<T>(this IQueryable<T> query, LimitOffsetPagingParameters pagingParameters)
            where T : class
        {
            int count = await query.CountAsync().ConfigureAwait(false);
            IReadOnlyCollection<T> result = await GetPageResult(query, pagingParameters);
            return new LimitOffsetPagingResult<T>(new LimitOffsetPaginationResult(pagingParameters, count, result.Count), result);
        }

        private static async Task<IReadOnlyCollection<T>> GetPageResult<T>(this IQueryable<T> query, LimitOffsetPagingParameters pagingParameters)
            where T : class
        {
            return await query
                .Skip(pagingParameters.Offset)
                .Take(pagingParameters.Limit)
                .ToListAsync()
                .ConfigureAwait(false);
        }
    }
}
