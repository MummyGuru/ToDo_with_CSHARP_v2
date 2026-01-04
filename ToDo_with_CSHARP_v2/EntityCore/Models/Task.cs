using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ToDo_with_CSHARP_v2.EntityCore.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int CategoryId { get; set; }
        public int StatusId { get; set; }
        public int PriorityId { get; set; }
        public bool IsCompleted { get; set; }
        public int Position { get; set; }
        public string SubTasksJson { get; set; }
        public bool HasDeadlineWarning { get; set; }

        [JsonIgnore]
        public Category Category { get; set; }

        [JsonIgnore]
        public Status Status { get; set; }

        [JsonIgnore]
        public Priority Priority { get; set; }

        [JsonIgnore]
        public List<Comment> Comments { get; set; } = new();

        [JsonIgnore]
        public bool IsOverdue => DueDate.HasValue && DueDate.Value.Date < DateTime.Today && !IsCompleted;

        [JsonIgnore]
        public bool IsDueToday => DueDate.HasValue && DueDate.Value.Date == DateTime.Today && !IsCompleted;

        [JsonIgnore]
        public bool IsDueTomorrow => DueDate.HasValue && DueDate.Value.Date == DateTime.Today.AddDays(1) && !IsCompleted;

        private List<SubTask> _subTasks;

        [JsonIgnore]
        public List<SubTask> SubTasks
        {
            get
            {
                if (_subTasks == null && !string.IsNullOrEmpty(SubTasksJson))
                {
                    try
                    {
                        _subTasks = JsonSerializer.Deserialize<List<SubTask>>(SubTasksJson) ?? new();
                    }
                    catch
                    {
                        _subTasks = new();
                    }
                }
                return _subTasks ?? new();
            }
            set
            {
                _subTasks = value;
                SubTasksJson = JsonSerializer.Serialize(value);
            }
        }
    }
}