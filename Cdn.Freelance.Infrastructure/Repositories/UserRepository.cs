using Cdn.Freelance.Domain.SeedWork;
using Cdn.Freelance.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Cdn.Freelance.Infrastructure.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly FreelanceContext _context;
        public UserRepository(FreelanceContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public User Add(User user)
        {
            return _context.Users.Add(user).Entity;
        }

        public void Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        public async Task<User?> FindAsync(string userIdentityGuid)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.IdentityGuid == userIdentityGuid);
        }

        public async Task<User?> FindByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}
