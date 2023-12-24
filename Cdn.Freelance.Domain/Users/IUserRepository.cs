using Cdn.Freelance.Domain.SeedWork;

namespace Cdn.Freelance.Domain.Users
{
    internal interface IUserRepository : IRepository
    {
        User Add(User user);
        void Update(User user);
        Task<LimitOffsetPagingResult<User>> GetAllUsersAsync(int limit, int offset);
        Task<User?> FindAsync(string userIdentityGuid);
        Task<User?> FindByIdAsync(int id);
        Task<bool> ExistsAsync(string username, string emailAddress);
        Task<bool> EmailAddressExistsAsync(string userNameToUpdate, string emailAddress);
        void DeleteAsync(User user);
    }
}
