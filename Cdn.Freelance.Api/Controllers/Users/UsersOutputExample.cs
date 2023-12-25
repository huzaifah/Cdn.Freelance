using Swashbuckle.AspNetCore.Filters;

namespace Cdn.Freelance.Api.Controllers.Users
{
    /// <summary>
    /// Users output example.
    /// </summary>
    public class UsersOutputExample : IExamplesProvider<LimitOffsetPagingResultModel<UserOutput>>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public LimitOffsetPagingResultModel<UserOutput> GetExamples()
        {
            return new LimitOffsetPagingResultModel<UserOutput>
            (
                new LimitOffsetPaginationResultModel(10, 2, 0, 2),
                new List<UserOutput>
                {
                    new UserOutputExample().GetExamples(),
                    new UserOutputExample().GetExamples()
                }.AsReadOnly()
            );
        }
    }

    /// <summary>
    /// User output example.
    /// </summary>
    public class UserOutputExample : IExamplesProvider<UserOutput>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public UserOutput GetExamples()
        {
            return new UserOutput()
            {
                IdentityGuid = "User Identifier",
                EmailAddress = "user@mail.com",
                PhoneNumber = "123123123",
                UserName = "john",
                Hobby = "Sailing",
                SkillSets = new List<string>() { "IOS", "Android App" }
            };
        }
    }
}
