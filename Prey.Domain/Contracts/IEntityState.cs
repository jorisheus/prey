using Prey.Domain.Entities.Contracts;

namespace Prey.Domain.Contracts
{
    public interface IEntityState
    {
        Point Position { get;  }
        IEntity Entity { get; }
        int Energy { get;  }
        int Age { get;  }
        IEntityDescription Description { get;  }
    }
}