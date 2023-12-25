using Cdn.Freelance.Domain.Users;
using FluentAssertions;
using FluentValidation;

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
        private const string LongText = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public UserTests()
        {
            _user = User.Build(IdentifierGuid, UserName, EmailAddress, PhoneNumber, Hobby);
        }

        [Fact]
        public void User_Build_ValidArguments_Ok()
        {
            _user.IdentityGuid.Should().Be(IdentifierGuid);
            _user.UserName.Should().Be(UserName);
            _user.EmailAddress.Should().Be(EmailAddress);
            _user.PhoneNumber.Should().Be(PhoneNumber);
            _user.Hobby.Should().Be(Hobby);
        }

        [Fact]
        public void User_Build_EmptyArguments_ThrowException()
        {
            Action action = () => User.Build("", "", "", "", "");
            action.Should().Throw<ValidationException>();
        }

        [Fact]
        public void User_Build_TextExceedMaxLength_ThrowException()
        {
            Action action = () => User.Build(LongText, LongText, LongText, LongText, LongText);
            action.Should().Throw<ValidationException>();
        }

        [Fact]
        public void User_Update_ValidArguments_Ok()
        {
            string updatedEmailAddress = "mark.dowle@outlook.com";
            string updatedPhoneNumber = "1234567890";

            _user.Update(updatedEmailAddress, updatedPhoneNumber, null);

            _user.EmailAddress.Should().Be(updatedEmailAddress);
            _user.PhoneNumber.Should().Be(updatedPhoneNumber);
            _user.Hobby.Should().BeNull();
        }

        [Fact]
        public void User_Update_EmptyArguments_ThrowException()
        {
            Action action = () => _user.Update("", "", "");
            action.Should().Throw<ValidationException>();
        }

        [Fact]
        public void User_WithNoHobby_Ok()
        {
            var userWithNoHobby = User.Build(IdentifierGuid, UserName, EmailAddress, PhoneNumber, null);
            userWithNoHobby.Hobby.Should().BeNull();
        }

        [Fact]
        public void User_UpdateSkillSets_Initialize_Ok()
        {
            var skillSets = new List<string> { SkillOne, SkillTwo };
            var user = User.Build(IdentifierGuid, UserName, EmailAddress, PhoneNumber, null);
            user.UpdateSkillSets(skillSets);

            user.SkillSets.Should().HaveCount(skillSets.Count);
            user.SkillSets.First().Skill.Should().Be(skillSets[0]);
            user.SkillSets.Last().Skill.Should().Be(skillSets[1]);
        }

        [Fact]
        public void User_UpdateSkillSets_NotEmpty_Ok()
        {
            var skillSets = new List<string> { SkillOne, SkillTwo };
            var user = User.Build(IdentifierGuid, UserName, EmailAddress, PhoneNumber, null);
            user.UpdateSkillSets(skillSets);

            skillSets.RemoveAt(1);
            user.UpdateSkillSets(skillSets);

            user.SkillSets.Should().HaveCount(1);
            user.SkillSets.First().Skill.Should().Be(skillSets[0]);
        }

        [Fact]
        public void User_UpdateSkillSets_NewSkillSets_Ok()
        {
            var skillSets = new List<string> { SkillOne, SkillTwo };
            var user = User.Build(IdentifierGuid, UserName, EmailAddress, PhoneNumber, null);
            user.UpdateSkillSets(skillSets);

            skillSets.Add(SkillThree);
            user.UpdateSkillSets(skillSets);

            user.SkillSets.Should().HaveCount(3);
            user.SkillSets.Should().ContainSingle(s => s.Skill == SkillThree);
        }

        [Fact]
        public void User_UpdateSkillSets_EmptyList_ThrowException()
        {
            Action action = () => _user.UpdateSkillSets(new List<string> { "", "" });
            action.Should().Throw<ValidationException>();
        }

        [Fact]
        public void User_RemoveExistingSkillSets_Ok()
        {
            var skillSets = new List<string> { SkillOne, SkillTwo };
            var user = User.Build(IdentifierGuid, UserName, EmailAddress, PhoneNumber, null);
            user.UpdateSkillSets(skillSets);
            user.RemoveExistingSkillSets();

            user.SkillSets.Should().BeEmpty();
        }
    }
}
