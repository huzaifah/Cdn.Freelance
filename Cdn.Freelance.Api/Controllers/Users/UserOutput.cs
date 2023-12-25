using Cdn.Freelance.Domain.Users;

namespace Cdn.Freelance.Api.Controllers.Users
{
    /// <summary>
    /// User information.
    /// </summary>
    public class UserOutput
    {
        /// <summary>
        /// User identifier.
        /// </summary>
        public string IdentityGuid { get; set; } = string.Empty;

        /// <summary>
        /// User name.
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// User's email address.
        /// </summary>
        public string EmailAddress { get; set; } = string.Empty;

        /// <summary>
        /// User's phone number.
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// User's hobby.
        /// </summary>
        public string? Hobby { get; set; }

        /// <summary>
        /// User's skill sets.
        /// </summary>
        public List<string> SkillSets { get; set; } = new();
    }

    internal static class UserExtensions
    {
        internal static UserOutput ToContract(this User domain)
        {
            var userOutput = new UserOutput
            {
                IdentityGuid = domain.IdentityGuid,
                UserName = domain.UserName,
                EmailAddress = domain.EmailAddress,
                PhoneNumber = domain.PhoneNumber,
                Hobby = domain.Hobby,
                SkillSets = domain.SkillSets.Select(s => s.Skill).Distinct().ToList()
            };

            return userOutput;
        }
    }
}
