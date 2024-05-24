

using TaskManager.Infrastructure.Persistence.Repositories;
using TaskMaster.Core.Domain.Entities;

namespace TaskMaster.Core.Application.Interfaces.Repositories
{
    public interface ITaskRepository : IGenericRepository<Tasks>
    {

    }
}
