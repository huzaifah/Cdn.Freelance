using Cdn.Freelance.Domain.SeedWork;

namespace Cdn.Freelance.Domain.Users
{
    internal interface IUserRepository : IRepository<User>
    {
        User Add(User user);
        void Update(User user);
        Task<User?> FindAsync(string userIdentityGuid);
        Task<User?> FindByIdAsync(int id);
    }
}
