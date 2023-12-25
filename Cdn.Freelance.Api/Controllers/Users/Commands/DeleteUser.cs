using Cdn.Freelance.Api.Exceptions;
using Cdn.Freelance.Domain.Users;
using MediatR;

namespace Cdn.Freelance.Api.Controllers.Users.Commands
{
    internal class DeleteUser
    {
        public class Command : IRequest<Unit>
        {
            public Command(string identifier)
            {
                Identifier = identifier;
            }

            public string Identifier { get; }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IUserRepository _userRepository;
            private readonly ILogger<Handler> _logger;

            public Handler(IUserRepository userRepository, ILogger<Handler> logger)
            {
                _userRepository = userRepository;
                _logger = logger;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var domain = await _userRepository.FindAsync(request.Identifier);

                if (domain is null)
                    throw new ItemNotFoundException(nameof(User), request.Identifier);

                _logger.LogInformation("Delete existing user {UserName}.", domain.UserName);

                _userRepository.Delete(domain);

                await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

                _logger.LogInformation("Existing user {UserName} deleted.", domain.UserName);

                return Unit.Value;
            }
        }
    }
}
