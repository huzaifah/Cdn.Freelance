using Cdn.Freelance.Domain.SeedWork;

namespace Cdn.Freelance.Domain.Users
{
    internal class SkillSet : Entity
    {
        public string Skill { get; set; }

        public SkillSet(string skill)
        {
            Skill = skill;
        }
    }
}
