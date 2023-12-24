namespace Cdn.Freelance.Domain.SeedWork
{
    public class LimitOffsetPaginationResult
    {
        /// <summary>
        /// Gets the results count
        /// </summary>
        public int ResultsCount { get; }

        /// <summary>
        /// Gets the Total results count
        /// </summary>
        public int TotalResultsCount { get; }

        /// <summary>
        /// Gets the offset
        /// </summary>
        public int Offset => LimitOffsetPagingParameters.Offset;

        /// <summary>
        /// Gets the limit
        /// </summary>
        public int Limit => LimitOffsetPagingParameters.Limit;

        /// <summary>
        /// Gets the limit offset parameters
        /// </summary>
        public LimitOffsetPagingParameters LimitOffsetPagingParameters { get; }

        /// <summary>
        /// Creates a new instance of <see cref="LimitOffsetPaginationResult"/>
        /// </summary>
        /// <param name="limitOffsetPagingParameters">The limit offset parameters</param>
        /// <param name="totalResultsCount">The total results count</param>
        /// <param name="resultsCount">The result count</param>
        public LimitOffsetPaginationResult(LimitOffsetPagingParameters limitOffsetPagingParameters, int totalResultsCount, int resultsCount)
        {
            LimitOffsetPagingParameters = limitOffsetPagingParameters;
            TotalResultsCount = totalResultsCount;
            ResultsCount = resultsCount;
        }
    }
}
