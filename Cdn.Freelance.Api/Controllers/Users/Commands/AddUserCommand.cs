using Cdn.Freelance.Domain.SeedWork;
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

        public class Handler : BaseHandler, IRequestHandler<Command, UserIdentifier>
        {
            private readonly IUserRepository _userRepository;

            public Handler(IUserRepository userRepository, IUnitOfWork unitOfWork, ILogger<BaseHandler> logger) : base(unitOfWork, logger)
            {
                _userRepository = userRepository;
            }

            public async Task<UserIdentifier> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = request.User;
                var userIdentifier = Guid.NewGuid().ToString();
                var domain = User.Build(userIdentifier, user.UserName, user.EmailAddress, user.PhoneNumber,
                    user.Hobby);

                if (user.SkillSets.Any())
                    domain.UpdateSkillSets(user.SkillSets.Distinct().ToList());

                _userRepository.Add(domain);

                await CommitAsync(nameof(User), userIdentifier, "Create");

                return new UserIdentifier() { Identifier = userIdentifier };
            }
        }
    }
}
