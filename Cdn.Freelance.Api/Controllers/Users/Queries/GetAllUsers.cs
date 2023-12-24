using Cdn.Freelance.Domain.Users;
using MediatR;

namespace Cdn.Freelance.Api.Controllers.Users.Queries
{
    internal class GetAllUsers
    {
        public class Query : IRequest<LimitOffsetPagingResultModel<UserOutput>>
        {
            public Query(int limit, int offset)
            {
                Limit = limit;
                Offset = offset;
            }

            public int Limit { get; }

            public int Offset { get; }
        }

        public class Handler : IRequestHandler<Query, LimitOffsetPagingResultModel<UserOutput>>
        {
            private readonly IUserRepository _userRepository;
            private readonly ILogger<Handler> _logger;

            public Handler(IUserRepository userRepository, ILogger<Handler> logger)
            {
                _userRepository = userRepository;
                _logger = logger;
            }

            public async Task<LimitOffsetPagingResultModel<UserOutput>> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _userRepository.GetAllUsersAsync(request.Limit, request.Offset);
                return result.MapToLimitOffsetPagingResultModel(u => u.ToContract());
            }
        }
    }
}
