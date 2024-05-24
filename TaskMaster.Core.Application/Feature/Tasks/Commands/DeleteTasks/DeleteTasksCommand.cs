using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net;
using TaskMaster.Core.Application.Exceptions;
using TaskMaster.Core.Application.Interfaces.Repositories;
using TaskMaster.Core.Application.Wrappers;

namespace TaskMaster.Core.Application.Feature.Tasks.Commands.DeleteTasks.TaskMaster.Core.Application.Features.Tasks.Commands.DeleteTask
{
    public class DeleteTaskCommand : IRequest<Response<int>>
    {
        [SwaggerParameter(Description = "ID of the task to be deleted")]
        [Required(ErrorMessage = "ID is required")]
        public int Id { get; set; }
    }

    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, Response<int>>
    {
        private readonly ITaskRepository _taskRepository;

        public DeleteTaskCommandHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<Response<int>> Handle(DeleteTaskCommand command, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(command.Id);

            if (task == null)
                throw new ApiException("Task not found", (int)HttpStatusCode.NotFound);

            _taskRepository.DeleteAsync(task.Id);

            return new Response<int>(task.Id);
        }
    }
}
