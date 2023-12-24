using System.Security.Claims;
using System.Security.Principal;
using System.Text.Json;

namespace Cdn.Freelance.Domain.SeedWork
{
    /// <summary>
    /// Provides access to the user  if one is available.
    /// </summary>
    public interface IUserAccessor
    {
        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <returns>The user.</returns>
        string GetUser();

        /// <summary>
        /// Assigns the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <exception cref="System.InvalidOperationException">Occurs when it's not possible to assign the user.</exception>
        void AssignUser(string user);
    }

    /// <summary>
    /// Represents the implementation of <see cref="IUserAccessor"/> using an <see cref="IPrincipal"/> to retrieve the user if not explicitly assigned.
    /// </summary>
    public class UserAccessor : IUserAccessor
    {
        /// <summary>
        /// The claim name related to the client_id.
        /// </summary>
        internal const string ClientIdClaim = "client_id";

        /// <summary>
        /// The claim name related to the subject.
        /// </summary>
        internal const string SubjectClaim = "sub";

        /// <summary>
        /// The claim name related to the actor.
        /// </summary>
        internal const string ActorClaim = "act";

        private readonly IPrincipal _principal;
        private readonly IUserComposer _userComposer;
        private bool _initialized;
        private string _user;

        private readonly object _lockObject = new object();


        /// <summary>
        /// Initializes a new instance of the <see cref="UserAccessor"/> class.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="userComposer">The user composer.</param>
        public UserAccessor(IPrincipal principal, IUserComposer userComposer)
        {
            _principal = principal;
            _userComposer = userComposer ?? throw new ArgumentNullException(nameof(userComposer));
        }


        /// <inheritdoc />
        public string GetUser()
        {
            //Simply for optimization and avoid lock when already initialized
            //=> we don't have code that set to false, so once it's true it won't change anymore,
            //if some code was able to set back to false we would have to lock also that if.
            if (!_initialized)
            {
                lock (_lockObject)
                {
                    //Ensure the initialized hasn't change between the first check and the lock.
                    if (!_initialized)
                    {
                        //if not initialized check if the correct claim is available otherwise thrown an exception.
                        if (!(_principal is ClaimsPrincipal claimPrincipal)) // If _principal == null then (_principal is ClaimsPrincipal) == false
                            throw new InvalidOperationException("Unable to retrieve the principal. You could be in a scope without \"IPrincipal\" of type \"ClaimsPrincipal\" (ex: Job and other). You can use \"AssignTenantIdentifier\" to force a system user.");

                        IEnumerable<Claim> claims = claimPrincipal.Claims;

                        Claim clientIdClaim = claims.FirstOrDefault(c => c.Type == ClientIdClaim);

                        if (clientIdClaim == null)
                        {
                            clientIdClaim = new Claim(ClientIdClaim, "Test");
                            //throw new InvalidOperationException("Unable to retrieve the user from the principal.");
                        }

                        Claim subjectClaim = claims.FirstOrDefault(c => c.Type == SubjectClaim)!;

                        Claim actClaim = claims.FirstOrDefault(c => c.Type == ActorClaim)!;
                        Dictionary<string, string> actorClaims = null;

                        if (actClaim != null)
                            actorClaims = JsonSerializer.Deserialize<Dictionary<string, string>>(actClaim.Value)!;

                        // Only the case of delegation for client_id is managed (not delegation on subject) because our auth server only manage this case.
                        _user = _userComposer.Compose(clientIdClaim.Value, subjectClaim?.Value, actorClaims?[ClientIdClaim]);
                        _initialized = true;
                    }
                }
            }
            return _user;
        }

        /// <inheritdoc />
        public void AssignUser(string user)
        {
            lock (_lockObject)
            {
                if (_initialized && !_user.Equals(user))
                    throw new InvalidOperationException("User can't be assigned when already initialized.");

                _user = user;
                _initialized = true;
            }
        }
    }
}
