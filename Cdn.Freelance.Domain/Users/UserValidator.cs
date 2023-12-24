using FluentValidation;

namespace Cdn.Freelance.Domain.Users
{
    internal class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.IdentityGuid).NotEmpty();
            RuleFor(user => user.EmailAddress).NotEmpty().MaximumLength(250).EmailAddress();
            RuleFor(user => user.Hobby).MaximumLength(250);
            RuleFor(user => user.PhoneNumber).NotEmpty().MaximumLength(50);
            RuleFor(user => user.UserName).NotEmpty().MaximumLength(250);
            RuleForEach(user => user.SkillSets).SetValidator(new SkillSetValidator());
        }
    }

    internal class SkillSetValidator : AbstractValidator<SkillSet>
    {
        public SkillSetValidator()
        {
            RuleFor(skillSet => skillSet.Skill).NotEmpty().MaximumLength(250);
        }
    }
}
