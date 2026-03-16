using System;

namespace ToDo_with_CSHARP_v2.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public int? CategoryId { get; set; }
        public int StatusId { get; set; }
        public DateTime? Deadline { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string StatusName { get; set; }
        public string CategoryName { get; set; }
        public string CategoryColor { get; set; }

        public bool IsOverdue =>
            Deadline.HasValue &&
            Deadline.Value < DateTime.Now &&
            StatusName != "Готово";
    }
}