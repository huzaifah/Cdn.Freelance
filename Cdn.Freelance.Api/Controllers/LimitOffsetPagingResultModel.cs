using Cdn.Freelance.Domain.SeedWork;

namespace Cdn.Freelance.Api.Controllers
{
    /// <summary>
    /// Represents the result model of a limit offset
    /// </summary>
    public class LimitOffsetPagingResultModel<T>
    {
        /// <summary>
        /// Gets the pagination
        /// </summary>
        public LimitOffsetPaginationResultModel Pagination { get; }

        /// <summary>
        /// Gets the results.
        /// </summary>
        /// <value>
        /// The results.
        /// </value>
        public IReadOnlyCollection<T> Results { get; }

        /// <summary>
        /// Creates a new instance of <see cref="LimitOffsetPagingResultModel{T}"/>
        /// </summary>
        /// <param name="pagination">The pagination</param>
        /// <param name="results">The list of results</param>
        public LimitOffsetPagingResultModel(LimitOffsetPaginationResultModel pagination, IReadOnlyCollection<T> results)
        {
            Pagination = pagination;
            Results = results;
        }
    }

    /// <summary>
    /// Represents the limit offset mapping extensions
    /// </summary>
    public static class LimitOffsetPagingMappingExtensions
    {
        /// <summary>
        /// Maps the LimitOffsetPagingResult to an LimitOffsetPagingResultModel.
        /// </summary>
        /// <typeparam name="T">The type of the pagingResult</typeparam>
        /// <typeparam name="TModel">The type of the pagingResult model.</typeparam>
        /// <param name="pageResult">The page pagingResult to map.</param>
        /// <param name="resultMappingFunc">The pagingResult mapping function (from T to TModel).</param>
        /// <returns>
        /// The mapped EndlessPageResultModel.
        /// </returns>
        public static LimitOffsetPagingResultModel<TModel> MapToLimitOffsetPagingResultModel<T, TModel>(this LimitOffsetPagingResult<T> pageResult, Func<T, TModel> resultMappingFunc)
            where T : class
        {
            return new LimitOffsetPagingResultModel<TModel>(new LimitOffsetPaginationResultModel(pageResult.Pagination.TotalResultsCount, pageResult.Pagination.ResultsCount,
                pageResult.Pagination.Offset, pageResult.Pagination.Limit), pageResult.Results.Select(resultMappingFunc).ToList());
        }
    }
}
