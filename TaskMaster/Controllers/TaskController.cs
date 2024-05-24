
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Net.Mime;
using TaskMaster.Core.Application.Exceptions;
using TaskMaster.Core.Application.Feature.Tasks.Commands.CreateTasks.TasksApp.Core.Application.Features.Tasks.Commands.CreateTask;
using TaskMaster.Core.Application.Feature.Tasks.Commands.DeleteTasks.TaskMaster.Core.Application.Features.Tasks.Commands.DeleteTask;
using TaskMaster.Core.Application.Feature.Tasks.Commands.UpdateTasks;
using TaskMaster.Core.Application.Feature.Tasks.Querys.GetAllTasks;
using TaskMaster.Core.Application.Feature.Tasks.Querys.GetTaskById;
using TaskMaster.Core.Application.FluentValidations;
using TaskMaster.WebApi.Controllers;

namespace TaskMaster.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [SwaggerTag("Task Manager")]
    public class TaskController : BaseApiController
    {
        public TaskController(IMediator mediator) : base(mediator)
        {
        }

        [Authorize(Roles = "Client")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes(MediaTypeNames.Application.Json)]
        [SwaggerOperation(
            Summary = "Get All Tasks",
            Description = "Retrieve all tasks"
        )]
        public async Task<IActionResult> List()
        {
            try
            {
                var result = await _mediator.Send(new GetAllTasksQuery());
                if (result == null || result.Data == null || result.Data.Count < 1)
                {
                    return NoContent();
                }
                return Ok(result);
            }
            catch (ApiException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "Client")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes(MediaTypeNames.Application.Json)]
        [SwaggerOperation(
            Summary = "Get Task by ID",
            Description = "Retrieve a task by its ID"
        )]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _mediator.Send(new GetTaskByIdQuery { Id = id });
                if (result == null || result.Data == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (ApiException ex) when (ex.ErrorCode == (int)HttpStatusCode.NotFound)
            {
                return NotFound(ex.Message);
            }
            catch (ApiException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "Client")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes(MediaTypeNames.Application.Json)]
        [SwaggerOperation(
       Summary = "Create a Task",
       Description = "Create a new task"
   )]
        public async Task<IActionResult> Create([FromBody] CreateTaskCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var validator = new CreateTaskCommandValidation();
            var validationResult = await validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { code = 400, error = errors });
            }

            var result = await _mediator.Send(command);
            if (result.Data == 0)
            {
                return BadRequest(new { code = 400, error = result.Message });
            }

            return CreatedAtAction(nameof(GetById), new { id = result.Data }, result);
        }

        [Authorize(Roles = "Client")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes(MediaTypeNames.Application.Json)]
                [SwaggerOperation(
            Summary = "Update a Task",
            Description = "Update an existing task"
        )]
        public async Task<IActionResult> Update([FromBody] UpdateTaskCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var validator = new UpdateTaskCommandValidation();
            var validationResult = await validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { code = 400, error = errors });
            }

            var result = await _mediator.Send(command);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }


        [Authorize(Roles = "Client")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes(MediaTypeNames.Application.Json)]
        [SwaggerOperation(
     Summary = "Delete a Task",
     Description = "Delete a task by its ID"
        )]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteTaskCommand { Id = id });
                if (result == null)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (ApiException ex) when (ex.ErrorCode == (int)HttpStatusCode.NotFound)
            {
                return NotFound(ex.Message);
            }
            catch (ApiException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
