using Swashbuckle.AspNetCore.Filters;

namespace Cdn.Freelance.Api.Controllers.Users
{
    /// <summary>
    /// User update example.
    /// </summary>
    public class UpdateUserInputExample : IExamplesProvider<UpdateUserInput>
    {
        /// <inheritdoc />
        public UpdateUserInput GetExamples()
        {
            return new UpdateUserInput()
            {
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
