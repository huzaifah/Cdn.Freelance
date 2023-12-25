using Cdn.Freelance.Api.Controllers.Users.Queries;
using Cdn.Freelance.Api.Exceptions;
using Cdn.Freelance.Domain.Users;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Cdn.Freelance.Api.Tests.Users.Queries
{
    public class GetUserByIdentifierTests
    {
        private readonly GetUserByIdentifier.Handler _handler;
        private readonly Mock<ILogger<GetUserByIdentifier.Handler>> _logger;
        private readonly Mock<IUserRepository> _userRepository;
        private const string Identifier = "Identity_One";
        private const string UserName = "UserOne";

        public GetUserByIdentifierTests()
        {
            _logger = new Mock<ILogger<GetUserByIdentifier.Handler>>();
            _userRepository = new Mock<IUserRepository>();

            _handler = new GetUserByIdentifier.Handler(_userRepository!.Object, _logger!.Object);
        }

        [Fact]
        public void Query_HappyFlow_Ok()
        {
            var query = new GetUserByIdentifier.Query(Identifier);
            query.Identifier.Should().Be(Identifier);
        }

        [Fact]
        public async Task Handler_HappyFlow_Ok()
        {
            var query = new GetUserByIdentifier.Query(Identifier);

            var user = User.Build(Identifier, UserName, "john@mail.com", "123123",
                null);

            _userRepository.Setup(u => u.FindAsync(Identifier)).ReturnsAsync(user);

            var actual = await _handler.Handle(query, CancellationToken.None);
            actual.IdentityGuid.Should().Be(Identifier);

            _userRepository.Verify(u => u.FindAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Handler_UserNotFound_ThrowsException()
        {
            var query = new GetUserByIdentifier.Query(Identifier);

            _userRepository.Setup(u => u.FindAsync(Identifier)).ReturnsAsync((User)null!);

            Func<Task> action = async () => await _handler.Handle(query, CancellationToken.None);
            await action.Should().ThrowAsync<ItemNotFoundException>();

            _userRepository.Verify(u => u.FindAsync(It.IsAny<string>()), Times.Once);
        }
    }
}
