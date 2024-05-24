using AutoMapper;
using TaskMaster.Core.Application.Dtos.Tasks;
using TaskMaster.Core.Application.Dtos.User;
using TaskMaster.Core.Application.Feature.Tasks.Commands.CreateTasks.TasksApp.Core.Application.Features.Tasks.Commands.CreateTask;
using TaskMaster.Core.Application.Feature.Tasks.Commands.UpdateTasks;
using TaskMaster.Core.Domain.Entities;

namespace TaskMaster.Core.Application.Mapping
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            #region
            CreateMap<CreateTaskCommand, Tasks>()
                 .ReverseMap();

            CreateMap<TaskUpdateResponse, Tasks>()
                 .ReverseMap();

            CreateMap<TaskDTO, Tasks>()
                 .ReverseMap();

            CreateMap<UpdateTaskCommand, Tasks>()
                 .ReverseMap();
            #endregion

        }
    }
}
