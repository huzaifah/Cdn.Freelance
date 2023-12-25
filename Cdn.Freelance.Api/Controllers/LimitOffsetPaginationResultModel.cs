namespace Cdn.Freelance.Api.Controllers
{
    /// <summary>
    /// Represents the result model of a pagination result
    /// </summary>
    public class LimitOffsetPaginationResultModel
    {
        /// <summary>
        /// Gets the Total results count
        /// </summary>
        public int TotalResultsCount { get; }

        /// <summary>
        /// Gets the results count
        /// </summary>
        public int ResultsCount { get; }

        /// <summary>
        /// Gets the offset
        /// </summary>
        public int Offset { get; }

        /// <summary>
        /// Gets the limit
        /// </summary>
        public int Limit { get; }

        /// <summary>
        /// Creates a new instance of <see cref="LimitOffsetPaginationResultModel"/>
        /// </summary>
        /// <param name="totalResultsCount">The total results count</param>
        /// <param name="resultsCount">The result count</param>
        /// <param name="offset">The offset</param>
        /// <param name="limit">The limit</param>
        public LimitOffsetPaginationResultModel(int totalResultsCount, int resultsCount, int offset, int limit)
        {
            TotalResultsCount = totalResultsCount;
            ResultsCount = resultsCount;
            Offset = offset;
            Limit = limit;
        }
    }
}
