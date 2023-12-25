using Cdn.Freelance.Domain.SeedWork;
using FluentValidation;

namespace Cdn.Freelance.Domain.Users
{
    internal class User : StampedEntity, IAggregateRoot
    {
        public string IdentityGuid { get; private set; }

        public string UserName { get; private set; }

        public string EmailAddress { get; private set; }
        
        public string PhoneNumber { get; private set; }

        public string? Hobby { get; private set; }

        public IEnumerable<SkillSet> SkillSets => _skillSets.AsReadOnly();

        private readonly List<SkillSet> _skillSets = new();

        public static User Build(string identityGuid, string userName, string emailAddress, string phoneNumber,
            string? hobby)
        {
            var user = new User(identityGuid, userName, emailAddress, phoneNumber, hobby);
            new UserValidator().ValidateAndThrow(user);

            return user;
        }

        protected User(string identityGuid, string userName, string emailAddress, string phoneNumber, string? hobby)
        {
            IdentityGuid = identityGuid;
            UserName = userName;
            EmailAddress = emailAddress;
            PhoneNumber = phoneNumber;
            
            if (!string.IsNullOrWhiteSpace(hobby))
                Hobby = hobby;
        }

        public void Update(string emailAddress, string phoneNumber, string? hobby)
        {
            EmailAddress = emailAddress;
            PhoneNumber = phoneNumber;
            Hobby = hobby;

            new UserValidator().ValidateAndThrow(this);
        }

        public void RemoveExistingSkillSets()
        {
            _skillSets.Clear();
        }

        public void UpdateSkillSets(List<string> skillSets)
        {
            var latestSkillSets = skillSets.ConvertAll(s => new SkillSet(s));

            if (_skillSets.Any())
            {
                // remove skill sets
                var removedSkillSets = _skillSets.Except(latestSkillSets, new SkillSetEqualityComparer()).ToList();
                foreach (var removedSkillSet in removedSkillSets)
                {
                    _skillSets.Remove(removedSkillSet);
                }
            }

            foreach (var skill in skillSets)
            {
                // add new skill set
                if (!_skillSets.Exists(s => s.Skill == skill))
                    _skillSets.Add(new SkillSet(skill));
            }

            new UserValidator().ValidateAndThrow(this);
        }
    }

    internal class SkillSetEqualityComparer : IEqualityComparer<SkillSet>
    {
        /// <inheritdoc />
        public bool Equals(SkillSet? x, SkillSet? y)
        {
            if (x == null || y == null)
                return false;

            return x.Skill == y.Skill;
        }

        /// <inheritdoc />
        public int GetHashCode(SkillSet obj)
        {
            return obj.Skill.GetHashCode();
        }
    }
}
