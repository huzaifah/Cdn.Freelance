using Cdn.Freelance.Api.Exceptions;
using Cdn.Freelance.Domain.Users;
using MediatR;

namespace Cdn.Freelance.Api.Controllers.Users.Commands
{
    internal class AddUser
    {
        public class Command : IRequest<UserIdentifier>
        {
            public Command(UserInput user)
            {
                User = user;
            }

            public UserInput User { get; }
        }

        public class Handler : IRequestHandler<Command, UserIdentifier>
        {
            private readonly IUserRepository _userRepository;
            private readonly ILogger<Handler> _logger;

            public Handler(IUserRepository userRepository, ILogger<Handler> logger)
            {
                _userRepository = userRepository;
                _logger = logger;
            }

            public async Task<UserIdentifier> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = request.User;

                _logger.LogInformation("Add new user {UserName}.", user.UserName);

                if (await _userRepository.ExistsAsync(user.UserName, user.EmailAddress))
                    throw new UserAlreadyExistsException($"User {user.UserName}, {user.EmailAddress} already exists.");

                var userIdentifier = Guid.NewGuid().ToString();
                var domain = User.Build(userIdentifier, user.UserName, user.EmailAddress, user.PhoneNumber,
                    user.Hobby);

                if (user.SkillSets.Any())
                    domain.UpdateSkillSets(user.SkillSets.Distinct().ToList());

                _userRepository.Add(domain);

                await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

                _logger.LogInformation("New user {UserName} created {UserIdentifier}.", user.UserName, userIdentifier);

                return new UserIdentifier() { Identifier = userIdentifier };
            }
        }
    }
}
