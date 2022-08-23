namespace Prey.Domain.Entities.Contracts
{
    public interface IAnimal : IEntity
    {

        IAnimal CreateBaby();

        Func<IConfiguration, IEntityDescription> GetBabyDescription();
    }
}