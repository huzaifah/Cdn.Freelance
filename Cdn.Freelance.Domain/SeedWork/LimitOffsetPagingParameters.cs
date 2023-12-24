namespace Cdn.Freelance.Domain.SeedWork
{
    public class LimitOffsetPagingParameters
    {
        /// <summary>
        /// Gets the limit of the paging
        /// </summary>
        public int Limit { get; }

        /// <summary>
        /// Gets the offset of the paging
        /// </summary>
        public int Offset { get; }
        
        /// <summary>
        /// Creates a new instance of <see cref="LimitOffsetPagingParameters"/>
        /// </summary>
        /// <param name="limit">The limit</param>
        /// <param name="offset">The offset</param>
        public LimitOffsetPagingParameters(int limit, int offset)
        {
            Limit = limit;
            Offset = offset;
        }
    }
}
