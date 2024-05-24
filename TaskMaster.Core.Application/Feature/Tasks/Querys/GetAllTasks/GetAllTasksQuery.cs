
using AutoMapper;
using MediatR;
using System.Net;
using TaskMaster.Core.Application.Dtos.Tasks;
using TaskMaster.Core.Application.Exceptions;
using TaskMaster.Core.Application.Interfaces.Repositories;
using TaskMaster.Core.Application.Wrappers;

namespace TaskMaster.Core.Application.Feature.Tasks.Querys.GetAllTasks
{
        public class GetAllTasksQuery : IRequest<Response<IList<TaskDTO>>>
        {
        }

        public class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, Response<IList<TaskDTO>>>
        {
            private readonly ITaskRepository _taskRepository;
            private readonly IMapper _mapper;

            public GetAllTasksQueryHandler(ITaskRepository taskRepository, IMapper mapper)
            {
                _taskRepository = taskRepository;
                _mapper = mapper;
            }

            public async Task<Response<IList<TaskDTO>>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
            {
                var tasks = await _taskRepository.GetAllAsync();

                if (tasks.Count == 0)
                    throw new ApiException("There is not Task Created", (int)HttpStatusCode.NoContent);

                var taskDTOs = _mapper.Map<IList<TaskDTO>>(tasks);

                return new Response<IList<TaskDTO>>(taskDTOs);
            }
        }

}

