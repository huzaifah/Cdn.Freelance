using Cdn.Freelance.Domain.SeedWork;
using Cdn.Freelance.Domain.Users;
using Cdn.Freelance.Infrastructure.Repositories;
using FluentAssertions;
using Moq;

namespace Cdn.Freelance.Infrastructure.Tests.Repositories
{
    public class UserRepositoryTests
    {
        private UserRepository? _repository;
        private FreelanceContext? _context;
        private Mock<IUserAccessor>? _userAccessor;
        private User? _userOne;
        private User? _userTwo;
        private User? _userThree;

        private async Task InitializeAsync()
        {
            _context = await InitializeContextHelper.InitializeAsync();
            await SetDataAsync(_context);
        }

        private async Task SetDataAsync(FreelanceContext context)
        {
            _userAccessor = new Mock<IUserAccessor>();
            _userAccessor.Setup(u => u.GetUser()).Returns("admin");
            _repository = new UserRepository(context, _userAccessor.Object);

            _userOne = User.Build("Identity_One", "mike", "mike@mail.com", "123213123", null);
            _userOne.UpdateSkillSets(new List<string>() { "C#", "Azure" });
            _repository.Add(_userOne);

            _userTwo = User.Build("Identity_Two", "rose", "rose@mail.com", "123213123", null);
            _userTwo.UpdateSkillSets(new List<string>() { "Java", "Oracle" });
            _repository.Add(_userTwo);

            _userThree = User.Build("Identity_Three", "jack", "jack@mail.com", "123213123", null);
            _userThree.UpdateSkillSets(new List<string>() { "COBOL", "DB2" });
            _repository.Add(_userThree);

            await context.SaveChangesInMemoryAsync();
        }

        [Fact]
        public async Task FindByUserName_ReturnCorrectUser()
        {
            await InitializeAsync();

            var user = await _repository!.FindAsync(_userOne!.IdentityGuid);
            user!.IdentityGuid.Should().Be(_userOne.IdentityGuid);
            user.UserName.Should().Be(_userOne.UserName);
            user.SkillSets.Should().HaveCount(_userOne.SkillSets.Count());
        }

        [Fact]
        public async Task GetAllUsers_ReturnCorrectUsers()
        {
            await InitializeAsync();

            var paginatedUsers = await _repository!.GetAllUsersAsync(10, 0);
            paginatedUsers.Results.Should().HaveCount(3);

            paginatedUsers.Results.Should().ContainEquivalentOf(_userOne);
            paginatedUsers.Results.Should().ContainEquivalentOf(_userTwo);
            paginatedUsers.Results.Should().ContainEquivalentOf(_userThree);
        }

        [Fact]
        public async Task Add_UserIsSaved()
        {
            await InitializeAsync();

            var user = User.Build("Identity_Four", "chris", "chris@mail.com", "546456456", null);
            _repository!.Add(user);
            await _context!.SaveChangesInMemoryAsync();

            var createdUser = await _repository.FindAsync("Identity_Four");

            createdUser!.IdentityGuid.Should().Be(user.IdentityGuid);
            createdUser.UserName.Should().Be(user.UserName);
            createdUser.SkillSets.Should().HaveCount(user.SkillSets.Count());
        }

        [Fact]
        public async Task Update_UserIsSaved()
        {
            await InitializeAsync();

            var user = User.Build("Identity_Five", "chris", "chris@mail.com", "546456456", null);
            _repository!.Add(user);
            await _context!.SaveChangesInMemoryAsync();

            var createdUser = await _repository.FindAsync("Identity_Five");
            createdUser!.Update("chris_edited@mail.com", "546456456", null);
            createdUser.UpdateSkillSets(new List<string>() { "IBM" });

            _repository.Update(createdUser);
            await _context!.SaveChangesInMemoryAsync();

            var updatedUser = await _repository.FindAsync("Identity_Five");
            updatedUser!.IdentityGuid.Should().Be(createdUser.IdentityGuid);
            updatedUser.EmailAddress.Should().Be(createdUser.EmailAddress);
            updatedUser.SkillSets.Should().HaveCount(createdUser.SkillSets.Count());
        }

        [Fact]
        public async Task Delete_UserIsDeleted()
        {
            await InitializeAsync();

            var user = User.Build("Identity_Six", "chris", "chris@mail.com", "546456456", null);
            _repository!.Add(user);
            await _context!.SaveChangesInMemoryAsync();

            var createdUser = await _repository.FindAsync("Identity_Six");
            createdUser.Should().NotBeNull();
            
            _repository.Delete(createdUser!);
            await _context!.SaveChangesInMemoryAsync();

            var updatedUser = await _repository.FindAsync("Identity_Six");
            updatedUser.Should().BeNull();
        }
    }
}
