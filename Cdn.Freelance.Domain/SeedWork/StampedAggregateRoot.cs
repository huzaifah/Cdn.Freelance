namespace Cdn.Freelance.Domain.SeedWork
{
    internal abstract class StampedEntity : Entity
    {
        public string CreatedBy { get; set; } = string.Empty;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.MinValue;

        public string ModifiedBy { get; set; } = string.Empty;

        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.MinValue;
    }
}
