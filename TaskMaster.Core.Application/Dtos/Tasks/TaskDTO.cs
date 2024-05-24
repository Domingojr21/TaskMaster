
namespace TaskMaster.Core.Application.Dtos.Tasks
{
    public class TaskDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public int UserId { get; set; }
    }
}
