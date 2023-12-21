using Cdn.Freelance.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cdn.Freelance.Infrastructure.EntityConfigurations
{
    internal class SkillSetEntityTypeConfiguration : IEntityTypeConfiguration<SkillSet>
    {
        public void Configure(EntityTypeBuilder<SkillSet> skillSetConfiguration)
        {
            skillSetConfiguration.ToTable("skillset");
        }
    }
}
