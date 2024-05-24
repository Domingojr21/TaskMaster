

namespace TaskMaster.Core.Domain.Entities
{
    public class Tasks
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; } 
        public bool isCompleted { get; set; } = false;
        public string UserName { get; set; } = null!;
    }
}
