using AutoMapper;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using TaskMaster.Core.Application.Dtos.Tasks;
using TaskMaster.Core.Application.Exceptions;
using TaskMaster.Core.Application.Interfaces.Repositories;
using TaskMaster.Core.Application.Wrappers;

namespace TaskMaster.Core.Application.Feature.Tasks.Querys.GetTaskById
{
        public class GetTaskByIdQuery : IRequest<Response<TaskDTO>>
        {
            [SwaggerParameter(Description = "Id de la tarea a obtener")]
            public int Id { get; set; }
        }

        public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, Response<TaskDTO>>
        {
            private readonly ITaskRepository _taskRepository;
            private readonly IMapper _mapper;

            public GetTaskByIdQueryHandler(ITaskRepository taskRepository, IMapper mapper)
            {
                _taskRepository = taskRepository;
                _mapper = mapper;
            }

            public async Task<Response<TaskDTO>> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
            {
                var task = await GetById(request.Id);

                if (task == null)
                    throw new ApiException("Task not found", (int)HttpStatusCode.NotFound);

                return new Response<TaskDTO>(task);
            }

            private async Task<TaskDTO> GetById(int id)
            {
                var task = await _taskRepository.GetByIdAsync(id);

                if (task == null)
                    throw new ApiException("Task not found", (int)HttpStatusCode.NotFound);

                var taskDTO = _mapper.Map<TaskDTO>(task);

                return taskDTO;
            }
        }
    
}
