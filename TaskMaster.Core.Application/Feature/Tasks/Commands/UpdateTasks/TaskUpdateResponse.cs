
namespace TaskMaster.Core.Application.Feature.Tasks.Commands.UpdateTasks
{
    public class TaskUpdateResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
    }
}
