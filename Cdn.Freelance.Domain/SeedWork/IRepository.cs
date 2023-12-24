namespace Cdn.Freelance.Domain.SeedWork;

public interface IRepository
{
    IUnitOfWork UnitOfWork { get; }
}