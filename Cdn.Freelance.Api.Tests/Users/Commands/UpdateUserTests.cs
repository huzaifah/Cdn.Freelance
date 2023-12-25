using Cdn.Freelance.Api.Controllers.Users;
using Cdn.Freelance.Api.Controllers.Users.Commands;
using Cdn.Freelance.Api.Exceptions;
using Cdn.Freelance.Domain.SeedWork;
using Cdn.Freelance.Domain.Users;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Cdn.Freelance.Api.Tests.Users.Commands
{
    public class UpdateUserTests
    {
        private readonly UpdateUser.Handler _handler;
        private readonly Mock<ILogger<UpdateUser.Handler>> _logger;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly UpdateUserInput _updateUserInput;
        private const string UserIdentifier = "Identifier_One";
        private const string UserName = "UserOne";

        public UpdateUserTests()
        {
            _logger = new Mock<ILogger<UpdateUser.Handler>>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _userRepository = new Mock<IUserRepository>();

            _userRepository.SetupGet(u => u.UnitOfWork).Returns(_unitOfWork.Object);

            _handler = new UpdateUser.Handler(_userRepository!.Object, _logger!.Object);

            _updateUserInput = new UpdateUserInput()
            {
                EmailAddress = "john@mail.com",
                PhoneNumber = "123213213",
                Hobby = "Football"
            };
        }

        [Fact]
        public void Command_HappyFlow_Ok()
        {
            var command = new UpdateUser.Command(UserIdentifier, _updateUserInput);
            command.Identifier.Should().Be(UserIdentifier);
            command.User.Should().Be(_updateUserInput);
        }

        [Fact]
        public async Task Handler_HappyFlow_Ok()
        {
            var command = new UpdateUser.Command(UserIdentifier, _updateUserInput);

            var user = User.Build(UserIdentifier, UserName, _updateUserInput.EmailAddress, _updateUserInput.PhoneNumber,
                _updateUserInput.Hobby);

            _userRepository.Setup(u => u.FindAsync(UserIdentifier)).ReturnsAsync(user);

            await _handler.Handle(command, CancellationToken.None);

            _userRepository.Verify(u => u.FindAsync(It.IsAny<string>()), Times.Once);
            _userRepository.Verify(u => u.Update(It.IsAny<User>()), Times.Once);
            _unitOfWork.Verify(u => u.SaveEntitiesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handler_EmailAlreadyExists_ThrowsException()
        {
            var command = new UpdateUser.Command(UserIdentifier, _updateUserInput);

            var user = User.Build(UserIdentifier, UserName, _updateUserInput.EmailAddress, _updateUserInput.PhoneNumber,
                _updateUserInput.Hobby);

            _userRepository.Setup(u => u.FindAsync(UserIdentifier)).ReturnsAsync(user);
            _userRepository.Setup(u => u.EmailAddressExistsAsync(UserName, _updateUserInput.EmailAddress)).ReturnsAsync(true);

            Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);
            await action.Should().ThrowAsync<UserAlreadyExistsException>();

            _userRepository.Verify(u => u.EmailAddressExistsAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
