

using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TaskMaster.Core.Application.FluentValidations;

namespace TaskMaster.Core.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            #region AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            #endregion

            #region MediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            #endregion

            #region ValidationsService
            services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateTaskCommandValidation>();
            services.AddValidatorsFromAssemblyContaining<UpdateTaskCommandValidation>();
            #endregion
        }
    }
}
