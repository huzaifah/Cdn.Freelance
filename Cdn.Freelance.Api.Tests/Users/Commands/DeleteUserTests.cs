using Cdn.Freelance.Api.Controllers.Users.Commands;
using Cdn.Freelance.Api.Exceptions;
using Cdn.Freelance.Domain.SeedWork;
using Cdn.Freelance.Domain.Users;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Cdn.Freelance.Api.Tests.Users.Commands
{
    public class DeleteUserTests
    {
        private readonly DeleteUser.Handler _handler;
        private readonly Mock<ILogger<DeleteUser.Handler>> _logger;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IUserRepository> _userRepository;
        private const string UserIdentifier = "Identifier_One";
        private const string UserName = "UserOne";
        private const string EmailAddress = "user@mail.com";
        private const string PhoneNumber = "123123123";

        public DeleteUserTests()
        {
            _logger = new Mock<ILogger<DeleteUser.Handler>>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _userRepository = new Mock<IUserRepository>();

            _userRepository.SetupGet(u => u.UnitOfWork).Returns(_unitOfWork.Object);

            _handler = new DeleteUser.Handler(_userRepository!.Object, _logger!.Object);
        }

        [Fact]
        public void Command_HappyFlow_Ok()
        {
            var command = new DeleteUser.Command(UserIdentifier);
            command.Identifier.Should().Be(UserIdentifier);
        }

        [Fact]
        public async Task Handler_HappyFlow_Ok()
        {
            var command = new DeleteUser.Command(UserIdentifier);

            var user = User.Build(UserIdentifier, UserName, EmailAddress, PhoneNumber, null);
            _userRepository.Setup(u => u.FindAsync(UserIdentifier)).ReturnsAsync(user);

            await _handler.Handle(command, CancellationToken.None);

            _userRepository.Verify(u => u.FindAsync(It.IsAny<string>()), Times.Once);
            _userRepository.Verify(u => u.Delete(It.IsAny<User>()), Times.Once);
            _unitOfWork.Verify(u => u.SaveEntitiesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handler_UserNotFound_ThrowsException()
        {
            var command = new DeleteUser.Command(UserIdentifier);

            _userRepository.Setup(u => u.FindAsync(UserIdentifier)).ReturnsAsync((User)null!);

            Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);
            await action.Should().ThrowAsync<ItemNotFoundException>();

            _userRepository.Verify(u => u.FindAsync(It.IsAny<string>()), Times.Once);
        }
    }
}
