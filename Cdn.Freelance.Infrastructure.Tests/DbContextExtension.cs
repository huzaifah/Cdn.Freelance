using Microsoft.EntityFrameworkCore;

namespace Cdn.Freelance.Infrastructure.Tests
{
    internal static class DbContextExtension
    {
        /// <summary>
        /// Detach the entities and saves all changes made to this context in memory database.
        /// Refer to this page for details: https://entityframeworkcore.com/knowledge-base/52740665/how-to-disable-eager-loading-when-using-inmemorydatabase.
        /// </summary>
        /// <param name="dbContext">DbContext.</param>
        /// <returns>Total affected rows.</returns>
        public static int SaveChangesInMemory(this DbContext dbContext)
        {
            var affectedRows = dbContext.SaveChanges();
            DetachEntitiesWhenSaving(dbContext);
            return affectedRows;
        }

        public static async Task<int> SaveChangesInMemoryAsync(this DbContext dbContext)
        {
            return await SaveChangesInMemoryAsync(dbContext, default);
        }

        /// <summary>
        /// Detach the entities and saves all changes made to this context in memory database.
        /// Refer to this page for details: https://entityframeworkcore.com/knowledge-base/52740665/how-to-disable-eager-loading-when-using-inmemorydatabase.
        /// </summary>
        /// <param name="dbContext">DbContext.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Total affected rows.</returns>
        public static async Task<int> SaveChangesInMemoryAsync(this DbContext dbContext, CancellationToken cancellationToken)
        {
            var affectedRows = await dbContext.SaveChangesAsync(cancellationToken);
            DetachEntitiesWhenSaving(dbContext);
            return affectedRows;
        }

        private static void DetachEntitiesWhenSaving(DbContext dbContext)
        {
            dbContext.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);
        }
    }
}
