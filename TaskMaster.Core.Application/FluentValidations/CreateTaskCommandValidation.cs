using FluentValidation;
using TaskMaster.Core.Application.Feature.Tasks.Commands.CreateTasks.TasksApp.Core.Application.Features.Tasks.Commands.CreateTask;

namespace TaskMaster.Core.Application.FluentValidations
{
    public class CreateTaskCommandValidation : AbstractValidator<CreateTaskCommand>
    {
        public CreateTaskCommandValidation()
        {
            RuleFor(x => x.Title)
                .NotEmpty().NotNull().WithMessage("Title can't be null or empty")
                .Must(title => title != "string").WithMessage("The title cannot be 'string'.");

            RuleFor(x => x.DueDate)
                .Must(DueDateValidation)
                .WithMessage("The due date must be greater than or equal to the current date.");

            RuleFor(x => x.UserName)
                .NotNull().WithMessage("The username cannot be null.")
                .NotEqual("string").WithMessage("The username cannot be 'string'.")
                .Must(userName => userName != "string").WithMessage("The username cannot be 'string'.");
        }

        #region PrivateMethods
        private bool DueDateValidation(DateTime dueDate)
        {
            return dueDate >= DateTime.Today;
        }
        #endregion
    }
}
