namespace Cdn.Freelance.Domain.SeedWork
{
    public class LimitOffsetPagingResult<T> where T : class
    {
        /// <summary>
        /// Gets the pagination
        /// </summary>
        public LimitOffsetPaginationResult Pagination { get; }

        /// <summary>
        /// Gets the results.
        /// </summary>
        /// <value>
        /// The results.
        /// </value>
        public IReadOnlyCollection<T> Results { get; }

        /// <summary>
        /// Creates a new instance of <see cref="LimitOffsetPagingResult{T}"/>
        /// </summary>
        /// <param name="limitOffsetPaginationResult">The pagination</param>
        /// <param name="results">The results</param>
        public LimitOffsetPagingResult(LimitOffsetPaginationResult limitOffsetPaginationResult, IReadOnlyCollection<T> results)
        {
            Pagination = limitOffsetPaginationResult;
            Results = results;
        }
    }
}
