using Swashbuckle.AspNetCore.Filters;

namespace Cdn.Freelance.Api.Controllers.Users
{
    /// <summary>
    /// User input example.
    /// </summary>
    public class UserInputExample : IExamplesProvider<UserInput>
    {
        /// <inheritdoc />
        public UserInput GetExamples()
        {
            return new UserInput()
            {
                UserName = "john",
                EmailAddress = "jack@mail.com",
                Hobby = "playing golf",
                PhoneNumber = "123123213",
                SkillSets = new List<string>()
                {
                    "Java",
                    "Oracle"
                }
            };
        }
    }
}
