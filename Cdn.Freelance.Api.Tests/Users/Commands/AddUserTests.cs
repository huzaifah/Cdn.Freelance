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
    public class AddUserTests
    {
        private readonly AddUser.Handler _handler;
        private readonly Mock<ILogger<AddUser.Handler>> _logger;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly UserInput _userInput;

        public AddUserTests()
        {
            _logger = new Mock<ILogger<AddUser.Handler>>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _userRepository = new Mock<IUserRepository>();

            _userRepository.SetupGet(u => u.UnitOfWork).Returns(_unitOfWork.Object);

            _handler = new AddUser.Handler(_userRepository!.Object, _logger!.Object);

            _userInput = new UserInput()
            {
                UserName = "john",
                EmailAddress = "john@mail.com",
                PhoneNumber = "123213213",
                Hobby = "Football"
            };
        }

        [Fact]
        public void Command_HappyFlow_Ok()
        {
            var command = new AddUser.Command(_userInput);
            command.User.Should().Be(_userInput);
        }

        [Fact]
        public async Task Handler_HappyFlow_Ok()
        {
            var command = new AddUser.Command(_userInput);
            
            var user = User.Build("Identity_One", _userInput.UserName, _userInput.EmailAddress, _userInput.PhoneNumber,
                _userInput.Hobby);

            _userRepository.Setup(u => u.Add(It.IsAny<User>())).Returns(user);

            var actual = await _handler.Handle(command, CancellationToken.None);
            actual.Identifier.Should().NotBeEmpty();

            _userRepository.Verify(u => u.Add(It.IsAny<User>()), Times.Once);
            _unitOfWork.Verify(u => u.SaveEntitiesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handler_UserNameAlreadyExists_ThrowsException()
        {
            var command = new AddUser.Command(_userInput);

            var user = User.Build("Identity_Two", _userInput.UserName, _userInput.EmailAddress, _userInput.PhoneNumber,
                _userInput.Hobby);

            _userRepository.Setup(u => u.ExistsAsync(user.UserName, user.EmailAddress)).ReturnsAsync(true);

            Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);
            await action.Should().ThrowAsync<UserAlreadyExistsException>();

            _userRepository.Verify(u => u.ExistsAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
