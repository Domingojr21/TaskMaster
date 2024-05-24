
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskMaster.Core.Application.Feature.Tasks.Querys.GetAllTasks;
using TaskMaster.Core.Application.Wrappers;
using FluentAssertions;
using MediatR;
using System.Net;
using TaskMaster.Controllers;
using TaskMaster.Core.Application.Dtos.Tasks;
using TaskMaster.Core.Application.Exceptions;
using TaskMaster.Core.Application.Feature.Tasks.Querys.GetTaskById;
using TaskMaster.Core.Application.Feature.Tasks.Commands.CreateTasks.TasksApp.Core.Application.Features.Tasks.Commands.CreateTask;
using TaskMaster.Core.Application.Feature.Tasks.Commands.DeleteTasks.TaskMaster.Core.Application.Features.Tasks.Commands.DeleteTask;
using TaskMaster.Core.Application.Feature.Tasks.Commands.UpdateTasks;
using TaskMaster.Core.Application.FluentValidations;


namespace TaskMaster.Tests
{
    public class TaskControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly TaskController _taskController;

        public TaskControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();

            var httpContext = new DefaultHttpContext();
            var controllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            _taskController = new TaskController(_mediatorMock.Object)
            {
                ControllerContext = controllerContext
            };
        }

        #region Tests for GetAll method
        [Fact]
        public async Task List_ReturnsOkResult_WhenTasksExist()
        {
            // Arrange
            var taskDTOs = new List<TaskDTO>
            {
                new TaskDTO {
                    Id = 1,
                    Title = "Test Task",
                    Description = "Description",
                    CreationDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(1),
                    IsCompleted = false,
                    UserId = 1
                }
            };
            var response = new Response<IList<TaskDTO>>(taskDTOs);

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllTasksQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _taskController.List();

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            okResult.Value.Should().BeEquivalentTo(response);
        }

        [Fact]
        public async Task List_ReturnsNoContentResult_WhenNoTasksExist()
        {
            // Arrange
            var response = new Response<IList<TaskDTO>>(new List<TaskDTO>());

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllTasksQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _taskController.List();

            // Assert
            var noContentResult = result as NoContentResult;
            noContentResult.Should().NotBeNull();
            noContentResult.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Fact]
        public async Task List_ReturnsNoContentResult_WhenResultDataIsNull()
        {
            // Arrange
            Response<IList<TaskDTO>> response = new Response<IList<TaskDTO>>(data: null);

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllTasksQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _taskController.List();

            // Assert
            var noContentResult = result as NoContentResult;
            noContentResult.Should().NotBeNull();
            noContentResult.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Fact]
        public async Task List_ReturnsInternalServerError_WhenApiExceptionIsThrown()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllTasksQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ApiException("Custom error message", (int)HttpStatusCode.InternalServerError));

            // Act
            var result = await _taskController.List();

            // Assert
            var objectResult = result as ObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            objectResult.Value.Should().Be("Custom error message");
        }

        [Fact]
        public async Task List_CallsMediatorWithGetAllTasksQuery()
        {
            // Arrange
            var response = new Response<IList<TaskDTO>>(new List<TaskDTO>());
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllTasksQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            await _taskController.List();

            // Assert
            _mediatorMock.Verify(m => m.Send(It.IsAny<GetAllTasksQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        #endregion

        #region Tests for GetById method

        [Fact]
        public async Task GetById_ReturnsOkResult_WhenTaskExists()
        {
            // Arrange
            var taskDTO = new TaskDTO
            {
                Id = 1,
                Title = "Test Task",
                Description = "Description",
                CreationDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(1),
                IsCompleted = false,
                UserId = 1
            };
            var response = new Response<TaskDTO>(taskDTO);

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetTaskByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _taskController.GetById(1);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            okResult.Value.Should().BeEquivalentTo(response);
        }

        [Fact]
        public async Task GetById_ReturnsNotFoundResult_WhenTaskDoesNotExist()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetTaskByIdQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ApiException("Task not found", (int)HttpStatusCode.NotFound));

            // Act
            var result = await _taskController.GetById(1);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            notFoundResult.Value.Should().Be("Task not found");
        }

        [Fact]
        public async Task GetById_ReturnsInternalServerError_WhenApiExceptionIsThrown()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetTaskByIdQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ApiException("Custom error message", (int)HttpStatusCode.InternalServerError));

            // Act
            var result = await _taskController.GetById(1);

            // Assert
            var objectResult = result as ObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            objectResult.Value.Should().Be("Custom error message");
        }

        [Fact]
        public async Task GetById_CallsMediatorWithCorrectQuery()
        {
            // Arrange
            var taskDTO = new TaskDTO
            {
                Id = 1,
                Title = "Test Task",
                Description = "Description",
                CreationDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(1),
                IsCompleted = false,
                UserId = 1
            };
            var response = new Response<TaskDTO>(taskDTO);

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetTaskByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            await _taskController.GetById(1);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.Is<GetTaskByIdQuery>(q => q.Id == 1), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region Tests for Create method

        [Fact]
        public async Task Create_ReturnsCreatedResult_WhenTaskIsCreated()
        {
            // Arrange
            var command = new CreateTaskCommand
            {
                Title = "Test Task",
                Description = "Description",
                DueDate = DateTime.Now.AddDays(1),
                UserName = "TestUser"
            };
            var response = new Response<int>(1);

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateTaskCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _taskController.Create(command);

            // Assert
            var createdResult = result as CreatedAtActionResult;
            createdResult.Should().NotBeNull();
            createdResult.StatusCode.Should().Be(StatusCodes.Status201Created);
            createdResult.Value.Should().BeEquivalentTo(response);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _taskController.ModelState.AddModelError("Title", "Title is required");

            // Act
            var result = await _taskController.Create(new CreateTaskCommand());

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenValidationFails()
        {
            // Arrange
            var command = new CreateTaskCommand { Title = "string", DueDate = DateTime.Now.AddDays(-1), UserName = "string" };
            var validator = new CreateTaskCommandValidation();
            var validationResult = await validator.ValidateAsync(command);

            // Act
            var result = await _taskController.Create(command);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            badRequestResult.Value.Should().BeEquivalentTo(new { code = 400, error = validationResult.Errors.Select(e => e.ErrorMessage).ToList() });
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenCommandFails()
        {
            // Arrange
            var command = new CreateTaskCommand
            {
                Title = "Test Task",
                Description = "Description",
                DueDate = DateTime.Now.AddDays(1),
                UserName = "TestUser"
            };
            var response = new Response<int>(0, "This user is not registed in the system");

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateTaskCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _taskController.Create(command);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            badRequestResult.Value.Should().BeEquivalentTo(new { code = 400, error = response.Message });
        }

#endregion

        #region Tests for Update method

        [Fact]
        public async Task Update_ReturnsOkResult_WhenTaskIsUpdated()
        {
            // Arrange
            var command = new UpdateTaskCommand
            {
                Id = 1,
                Title = "Updated Task",
                Description = "Updated Description",
                DueDate = DateTime.Now.AddDays(1),
            };

            var taskUpdateResponse = new TaskUpdateResponse
            {
                Id = 1,
                Title = "Updated Task",
                Description = "Updated Description",
                DueDate = DateTime.Now.AddDays(1)
            };

            var response = new Response<TaskUpdateResponse>(taskUpdateResponse);

            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateTaskCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _taskController.Update(command);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            okResult.Value.Should().BeEquivalentTo(response);
        }


        [Fact]
        public async Task Update_ReturnsNotFound_WhenTaskDoesNotExist()
        {
            // Arrange
            var command = new UpdateTaskCommand
            {
                Id = 1,
                Title = "Updated Task",
                Description = "Updated Description",
                DueDate = DateTime.Now.AddDays(1),
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateTaskCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Response<TaskUpdateResponse>)null);

            // Act
            var result = await _taskController.Update(command);

            // Assert
            var notFoundResult = result as NotFoundResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }


        [Fact]
        public async Task Update_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _taskController.ModelState.AddModelError("Title", "Title is required");

            // Act
            var result = await _taskController.Update(new UpdateTaskCommand());

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenValidationFails()
        {
            // Arrange
            var command = new UpdateTaskCommand { Id = 1, Title = "string", DueDate = DateTime.Now.AddDays(-1)};
            var validator = new UpdateTaskCommandValidation();
            var validationResult = await validator.ValidateAsync(command);

            // Act
            var result = await _taskController.Update(command);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            badRequestResult.Value.Should().BeEquivalentTo(new { code = 400, error = validationResult.Errors.Select(e => e.ErrorMessage).ToList() });
        }
        #endregion

        #region Tests for Delete method

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenTaskIsDeleted()
        {
            // Arrange
            var response = new Response<int>(1);

            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteTaskCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _taskController.Delete(1);

            // Assert
            var noContentResult = result as NoContentResult;
            noContentResult.Should().NotBeNull();
            noContentResult.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenTaskDoesNotExist()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteTaskCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ApiException("Task not found", (int)HttpStatusCode.NotFound));

            // Act
            var result = await _taskController.Delete(1);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            notFoundResult.Value.Should().Be("Task not found");
        }
        #endregion
    }
}
