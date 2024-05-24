using FluentValidation;
using TaskMaster.Core.Application.Feature.Tasks.Commands.UpdateTasks;

namespace TaskMaster.Core.Application.FluentValidations
{
    public class UpdateTaskCommandValidation : AbstractValidator<UpdateTaskCommand>
    {
        public UpdateTaskCommandValidation()
        {
            RuleFor(x => x.Title)
                .NotEmpty().NotNull().WithMessage("Title can't be null or empty")
                .Must(title => title != "string").WithMessage("The title cannot be 'string'.");

            RuleFor(x => x.DueDate)
                .Must(DueDateValidation)
                .WithMessage("The due date must be greater than or equal to the current date.");
        }

        #region PrivateMethods
        private bool DueDateValidation(DateTime dueDate)
        {
            return dueDate >= DateTime.Today;
        }
        #endregion
    }
}
