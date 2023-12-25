using System.ComponentModel.DataAnnotations;

namespace Cdn.Freelance.Api.Controllers.Users
{
    /// <summary>
    /// User input.
    /// </summary>
    public class UserInput
    {
        /// <summary>
        /// User name.
        /// </summary>
        [Required, MaxLength(250)]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// User's email address.
        /// </summary>
        [Required, EmailAddress, MaxLength(250)]
        public string EmailAddress { get; set; } = string.Empty;

        /// <summary>
        /// User's phone number.
        /// </summary>
        [Required, Phone, MaxLength(50)]
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// User's hobby.
        /// </summary>
        [MaxLength(250)]
        public string? Hobby { get; set; }

        /// <summary>
        /// User's skill sets.
        /// </summary>
        public List<string> SkillSets { get; set; } = new ();
    }
}
