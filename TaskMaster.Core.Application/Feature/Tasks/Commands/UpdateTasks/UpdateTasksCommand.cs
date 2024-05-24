using AutoMapper;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net;
using TaskMaster.Core.Application.Exceptions;
using TaskMaster.Core.Application.Interfaces.Repositories;
using TaskMaster.Core.Application.Wrappers;

namespace TaskMaster.Core.Application.Feature.Tasks.Commands.UpdateTasks
{
    public class UpdateTaskCommand : IRequest<Response<TaskUpdateResponse>>
    {
        /// <summary>
        /// ID of the task to be updated.
        /// </summary>
        [Required(ErrorMessage = "ID is required")]
        [SwaggerParameter(Description = "ID of the task to be updated")]
        public int Id { get; set; }

        /// <summary>
        /// New title of the task.
        /// </summary>
        [Required(ErrorMessage = "Title is required")]
        [SwaggerParameter(Description = "New title of the task")]
        public string Title { get; set; } = null!;

        /// <summary>
        /// New description of the task.
        /// </summary>
        [SwaggerParameter(Description = "New description of the task")]
        public string? Description { get; set; }

        /// <summary>
        /// New due date of the task.
        /// </summary>
        [Required(ErrorMessage = "Due date is required")]
        [SwaggerParameter(Description = "New due date of the task")]
        public DateTime DueDate { get; set; }
    }

    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, Response<TaskUpdateResponse>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public UpdateTaskCommandHandler(ITaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        public async Task<Response<TaskUpdateResponse>> Handle(UpdateTaskCommand command, CancellationToken cancellationToken)
        {
            var oldTask = await _taskRepository.GetByIdAsync(command.Id);

            if (oldTask == null)
                throw new ApiException("Task not found", (int)HttpStatusCode.NotFound);

            var task = _mapper.Map<Domain.Entities.Tasks>(command);

            task.UserName = oldTask.UserName;

            await _taskRepository.UpdateAsync(task.Id,task);

            var response = _mapper.Map<TaskUpdateResponse>(task);

            return new Response<TaskUpdateResponse>(response);
        }
    }

}
