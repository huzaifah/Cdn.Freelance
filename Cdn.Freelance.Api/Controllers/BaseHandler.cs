using Cdn.Freelance.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace Cdn.Freelance.Api.Controllers
{
    /// <summary>
    /// BaseHandler
    /// </summary>
    public abstract class BaseHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        /// <summary>
        /// Logger
        /// </summary>
        protected readonly ILogger<BaseHandler> Logger;

        /// <summary>
        /// BaseHandler constructor
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="logger"></param>
        protected BaseHandler(IUnitOfWork unitOfWork, ILogger<BaseHandler> logger)
        {
            _unitOfWork = unitOfWork;
            Logger = logger;
        }

        /// <summary>
        /// Commit changes
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="identifier"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        protected async Task CommitAsync(string entityName, object identifier, string action)
        {
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                //NOTE: We log and rethrow the inner exception here because it has clearer information.
                var exception = ex.InnerException ?? ex;
                Logger?.LogError(exception, "DB update error during {Action} of {EntityName} {Identifier}", action, entityName, identifier);
                throw exception;
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, "Error during {Action} of {EntityName} {Identifier}", action, entityName, identifier);
                throw;
            }
        }
    }
}
