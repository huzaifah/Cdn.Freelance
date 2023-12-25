using Cdn.Freelance.Api.Exceptions;
using Cdn.Freelance.Domain.Users;
using MediatR;

namespace Cdn.Freelance.Api.Controllers.Users.Queries
{
    internal class GetUserByIdentifier
    {
        public class Query : IRequest<UserOutput>
        {
            public Query(string identifier)
            {
                Identifier = identifier;
            }

            public string Identifier { get; }
        }

        public class Handler : IRequestHandler<Query, UserOutput>
        {
            private readonly IUserRepository _userRepository;
            private readonly ILogger<Handler> _logger;

            public Handler(IUserRepository userRepository, ILogger<Handler> logger)
            {
                _userRepository = userRepository;
                _logger = logger;
            }

            public async Task<UserOutput> Handle(Query request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Get user information {UserIdentifier}.", request.Identifier);

                var user = await _userRepository.FindAsync(request.Identifier);

                if (user is null)
                    throw new ItemNotFoundException(nameof(User), request.Identifier);

                return user.ToContract();
            }
        }
    }
}
