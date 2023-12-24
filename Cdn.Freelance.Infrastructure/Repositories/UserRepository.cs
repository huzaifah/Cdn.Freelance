using Cdn.Freelance.Domain.SeedWork;
using Cdn.Freelance.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Cdn.Freelance.Infrastructure.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly FreelanceContext _context;
        private readonly IUserAccessor _userAccessor;

        public UserRepository(FreelanceContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public IUnitOfWork UnitOfWork => _context;

        public User Add(User user)
        {
            _context.Users.Add(user);
            BeforeSaveChanges();
            return user;
        }

        public void Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            BeforeSaveChanges();
        }

        public async Task<LimitOffsetPagingResult<User>> GetAllUsersAsync(int limit, int offset)
        {
            return await _context.Users.Include(u => u.SkillSets).OrderBy(u => u.UserName).Paginate(new LimitOffsetPagingParameters(limit, offset));
        }

        public async Task<User?> FindAsync(string userIdentityGuid)
        {
            return await _context.Users.Include(u => u.SkillSets).FirstOrDefaultAsync(u => u.IdentityGuid == userIdentityGuid);
        }

        public async Task<User?> FindByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<bool> ExistsAsync(string username, string emailAddress)
        {
            return await _context.Users.AnyAsync(u => 
                u.UserName.ToLower() == username.ToLower() || 
                u.EmailAddress == emailAddress);
        }

        protected void BeforeSaveChanges()
        {
            var now = DateTime.UtcNow;
            var user = _userAccessor.GetUser();

            // Entities which are going to be inserted.
            GetEntities(_context, EntityState.Added).ForEach(c =>
            {
                c.CreatedAt = now;
                c.CreatedBy = user;
                c.ModifiedAt = now;
                c.ModifiedBy = user;
            });

            // Entities which are going to be updated.
            GetEntities(_context, EntityState.Modified).ForEach(c =>
            {
                c.ModifiedAt = now;
                c.ModifiedBy = user;
            });
        }

        private static List<StampedEntity> GetEntities(DbContext context, EntityState state)
        {
            return context.ChangeTracker.Entries<StampedEntity>()
                .Where(x => x.State == state)
                .Select(c => c.Entity)
                .ToList();
        }
    }
}
