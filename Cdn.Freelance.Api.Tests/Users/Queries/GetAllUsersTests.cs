using Cdn.Freelance.Api.Controllers.Users.Queries;
using Cdn.Freelance.Domain.SeedWork;
using Cdn.Freelance.Domain.Users;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Cdn.Freelance.Api.Tests.Users.Queries
{
    public class GetAllUsersTests
    {
        private readonly GetAllUsers.Handler _handler;
        private readonly Mock<ILogger<GetAllUsers.Handler>> _logger;
        private readonly Mock<IUserRepository> _userRepository;
        private const int Limit = 10;

        public GetAllUsersTests()
        {
            _logger = new Mock<ILogger<GetAllUsers.Handler>>();
            _userRepository = new Mock<IUserRepository>();

            _handler = new GetAllUsers.Handler(_userRepository!.Object, _logger!.Object);
        }

        [Fact]
        public void Query_HappyFlow_Ok()
        {
            var query = new GetAllUsers.Query(Limit, 0);
            query.Limit.Should().Be(Limit);
            query.Offset.Should().Be(0);
        }

        [Fact]
        public async Task Handler_HappyFlow_Ok()
        {
            var query = new GetAllUsers.Query(Limit, 0);

            var user = User.Build("Identity_One", "john", "john@mail.com", "123123",
                null);

            _userRepository.Setup(u => u.GetAllUsersAsync(Limit, 0)).ReturnsAsync(new LimitOffsetPagingResult<User>(
                new LimitOffsetPaginationResult(new LimitOffsetPagingParameters(Limit, 0), 1, 1), 
                new List<User> { user }));

            var actual = await _handler.Handle(query, CancellationToken.None);
            actual.Results.Should().HaveCount(1);
            actual.Pagination.Limit.Should().Be(Limit);

            _userRepository.Verify(u => u.GetAllUsersAsync(Limit, 0), Times.Once);
        }
    }
}
