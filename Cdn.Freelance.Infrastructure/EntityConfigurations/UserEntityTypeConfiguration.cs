using Cdn.Freelance.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cdn.Freelance.Infrastructure.EntityConfigurations
{
    internal class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> userConfiguration)
        {
            userConfiguration.ToTable("users");
            userConfiguration.Ignore(u => u.DomainEvents);
            userConfiguration.Property(u => u.Id).UseHiLo("userseq");
            
            userConfiguration.Property(u => u.IdentityGuid).HasMaxLength(250).IsRequired();
            userConfiguration.Property(u => u.EmailAddress).HasMaxLength(250).IsRequired();
            userConfiguration.Property(u => u.UserName).HasMaxLength(250).IsRequired();
            userConfiguration.Property(u => u.PhoneNumber).HasMaxLength(50).IsRequired();
            userConfiguration.Property(u => u.Hobby).HasMaxLength(250);

            userConfiguration.HasKey(u => u.Id);
            userConfiguration.HasIndex(u => u.IdentityGuid).IsUnique();

            userConfiguration.HasMany(u => u.SkillSets).WithOne();
        }
    }
}
