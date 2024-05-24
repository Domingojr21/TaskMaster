using AutoMapper;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using TaskMaster.Core.Application.Interfaces.Repositories;
using TaskMaster.Core.Application.Wrappers;
using TaskMaster.Infraestructure.Identity.Services;

namespace TaskMaster.Core.Application.Feature.Tasks.Commands.CreateTasks.TasksApp.Core.Application.Features.Tasks.Commands.CreateTask
{
    public class CreateTaskCommand : IRequest<Response<int>>
    {
        [Required(ErrorMessage = "Title is required")]
        [SwaggerParameter(Description = "New title of the task")]
        public string Title { get; set; } = null!;

        [SwaggerParameter(Description = "New description of the task")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Due date is required")]
        [SwaggerParameter(Description = "New due date of the task")]
        public DateTime DueDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "UserName is required")]
        [SwaggerParameter(Description = "This is the username of the owner of the task.")]
        public string UserName { get; set; } = null!;
    }

    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Response<int>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;

        public CreateTaskCommandHandler(ITaskRepository taskRepository, IMapper mapper, IAccountService accountService)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
            _accountService = accountService;
        }

        public async Task<Response<int>> Handle(CreateTaskCommand command, CancellationToken cancellationToken)
        {
            bool userIsValid = await _accountService.VerifyUser(command.UserName);

            if (!userIsValid)
            {
                return new Response<int>(0, "This user is not registed in the system");
            }

            var task = _mapper.Map<Domain.Entities.Tasks>(command);
            task = await _taskRepository.AddAsync(task);

            return new Response<int>(task.Id);
        }
    }
}
