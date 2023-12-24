using Swashbuckle.AspNetCore.Filters;

namespace Cdn.Freelance.Api.Controllers
{
    /// <summary>
    /// User identifier example.
    /// </summary>
    public class UserIdentifierExample : IExamplesProvider<UserIdentifier>
    {
        /// <summary>
        /// Set example.
        /// </summary>
        /// <returns>Model example.</returns>
        public UserIdentifier GetExamples()
        {
            return new UserIdentifier()
            {
                Identifier = "User identifier"
            };
        }
    }
}
