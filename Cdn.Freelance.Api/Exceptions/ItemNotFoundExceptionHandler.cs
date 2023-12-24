using System.Net;

namespace Cdn.Freelance.Api.Exceptions
{
    /// <summary>
    /// Handle user already exist exception. 
    /// </summary>
    public class ItemNotFoundExceptionHandler : BaseExceptionHandler<ItemNotFoundException>
    {
        /// <inheritdoc />
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        /// <inheritdoc />
        public override string ErrorMessage => "Item not found.";

        /// <inheritdoc />
        public ItemNotFoundExceptionHandler(ILogger<BaseExceptionHandler<ItemNotFoundException>> logger) : base(logger)
        {
        }
    }

    /// <summary>
    /// Item not found exception.
    /// </summary>
    public class ItemNotFoundException : Exception
    {
        /// <inheritdoc />
        public ItemNotFoundException(string? message) : base(message)
        {
        }
        /// <inheritdoc />
        public ItemNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemNotFoundException"/> class.
        /// </summary>
        /// <param name="entityName">The entity.</param>
        /// <param name="identifier">The identifier.</param>
        public ItemNotFoundException(string entityName, object identifier) : base($"The {entityName} item {identifier} could not be found.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemNotFoundException"/> class.
        /// </summary>
        /// <param name="entityName">The entity.</param>
        /// <param name="identifiers">The identifiers.</param>
        public ItemNotFoundException(string entityName, IEnumerable<object> identifiers) : base($"Those {entityName} items could not be found : {string.Join(" - ", identifiers)}.")
        {
        }
    }
}
