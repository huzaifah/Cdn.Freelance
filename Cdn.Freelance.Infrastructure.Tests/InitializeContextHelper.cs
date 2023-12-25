using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using System.Security.Principal;

namespace Cdn.Freelance.Infrastructure.Tests
{
    internal static class InitializeContextHelper
    {
        public static async Task<FreelanceContext> InitializeAsync()
        {
            var options = new DbContextOptionsBuilder<FreelanceContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            return await InitializeAsync(options, true);
        }

        public static async Task<FreelanceContext> InitializeAsync(DbContextOptions<FreelanceContext> options, bool resetDatabase)
        {
            var claimsIdentities = new List<ClaimsIdentity>();
            var principal = new ClaimsPrincipal(claimsIdentities);
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<IPrincipal>(c => principal);

            var context = new FreelanceContext(options);

            if (resetDatabase)
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
            }
            return context;
        }
    }
}
