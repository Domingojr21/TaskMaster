

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Infrastructure.Persistence.Context;
using TaskManager.Infrastructure.Persistence.Repositories;
using TaskMaster.Core.Application.Interfaces.Repositories;

namespace TaskManager.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceLayer(this IServiceCollection service, IConfiguration configuration)
        {
            #region context
            service.AddDbContext<ApplicationContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("AppConnectionString"),
            m => m.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName))); 
                
            #endregion

            #region Services
            service.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            service.AddTransient<ITaskRepository, TaskRepository>();
            #endregion
        }
    }
}
