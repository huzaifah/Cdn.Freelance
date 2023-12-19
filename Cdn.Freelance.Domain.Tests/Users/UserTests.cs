using Cdn.Freelance.Domain.Users;
using FluentAssertions;

namespace Cdn.Freelance.Domain.Tests.Users
{   
    public class UserTests
    {
        private readonly User _user;
        private const string IdentifierGuid = "11111";
        private const string UserName = "mark.dowle";
        private const string EmailAddress = "mark.dowle@outlook.com";
        private const string PhoneNumber = "1234567890";
        private const string Hobby = "Football";
        private const string SkillOne = "Skill One";
        private const string SkillTwo = "Skill Two";
        private const string SkillThree = "Skill Three";

        public UserTests()
        {
            _user = new User(IdentifierGuid, UserName, EmailAddress, PhoneNumber, Hobby);
        }

        [Fact]
        public void User_ValidArguments_Ok()
        {
            _user.IdentityGuid.Should().Be(IdentifierGuid);
            _user.UserName.Should().Be(UserName);
            _user.EmailAddress.Should().Be(EmailAddress);
            _user.PhoneNumber.Should().Be(PhoneNumber);
            _user.Hobby.Should().Be(Hobby);
        }

        [Fact]
        public void User_WithNoHobby_Ok()
        {
            var userWithNoHobby = new User(IdentifierGuid, UserName, EmailAddress, PhoneNumber, null);
            userWithNoHobby.Hobby.Should().BeNull();
        }

        [Fact]
        public void User_InitializeSkillSets_Ok()
        {
            var skillSets = new List<string> { SkillOne, SkillTwo };
            var user = new User(IdentifierGuid, UserName, EmailAddress, PhoneNumber, null);
            user.UpdateSkillSets(skillSets);

            user.SkillSets.Should().HaveCount(skillSets.Count);
            user.SkillSets.First().Skill.Should().Be(skillSets[0]);
            user.SkillSets.Last().Skill.Should().Be(skillSets[1]);
        }

        [Fact]
        public void User_RemoveExistingSkillSets_Ok()
        {
            var skillSets = new List<string> { SkillOne, SkillTwo };
            var user = new User(IdentifierGuid, UserName, EmailAddress, PhoneNumber, null);
            user.UpdateSkillSets(skillSets);

            skillSets.RemoveAt(1);
            user.UpdateSkillSets(skillSets);

            user.SkillSets.Should().HaveCount(1);
            user.SkillSets.First().Skill.Should().Be(skillSets[0]);
        }

        [Fact]
        public void User_AddNewSkillSets_Ok()
        {
            var skillSets = new List<string> { SkillOne, SkillTwo };
            var user = new User(IdentifierGuid, UserName, EmailAddress, PhoneNumber, null);
            user.UpdateSkillSets(skillSets);

            skillSets.Add(SkillThree);
            user.UpdateSkillSets(skillSets);

            user.SkillSets.Should().HaveCount(3);
            user.SkillSets.Should().ContainSingle(s => s.Skill == SkillThree);
        }
    }
}
