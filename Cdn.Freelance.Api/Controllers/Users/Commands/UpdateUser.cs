using Cdn.Freelance.Api.Exceptions;
using Cdn.Freelance.Domain.Users;
using MediatR;

namespace Cdn.Freelance.Api.Controllers.Users.Commands
{
    internal class UpdateUser
    {
        public class Command : IRequest<Unit>
        {
            public Command(string identifier, UpdateUserInput user)
            {
                User = user;
                Identifier = identifier;
            }

            public string Identifier { get; }

            public UpdateUserInput User { get; }
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
                var user = request.User;

                var domain = await _userRepository.FindAsync(request.Identifier);

                if (domain is null)
                    throw new ItemNotFoundException(nameof(User), request.Identifier);

                _logger.LogInformation("Update existing user {UserName}.", domain.UserName);

                if (await _userRepository.EmailAddressExistsAsync(domain.UserName, user.EmailAddress))
                    throw new UserAlreadyExistsException($"User {user.EmailAddress} already exists.");

                domain.Update(user.EmailAddress, user.PhoneNumber, user.Hobby);

                if (user.SkillSets.Any())
                    domain.UpdateSkillSets(user.SkillSets.Distinct().ToList());
                else
                    domain.RemoveExistingSkillSets();

                _userRepository.Update(domain);

                await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

                _logger.LogInformation("Existing user {UserName} updated.", domain.UserName);

                return Unit.Value;
            }
        }
    }
}
