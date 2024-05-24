
using TaskManager.Infrastructure.Persistence.Context;
using TaskMaster.Core.Application.Interfaces.Repositories;
using TaskMaster.Core.Domain.Entities;

namespace TaskManager.Infrastructure.Persistence.Repositories
{
    public class TaskRepository : GenericRepository<Tasks>, ITaskRepository
    {
        public readonly ApplicationContext _dbContext;

        public TaskRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
