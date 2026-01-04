using System;
using System.Collections.Generic;
using System.Linq;
using ToDo_with_CSHARP_v2.EntityCore.Repository.Interfaces;

namespace BSLogic
{
    public class TaskManagerService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IStatusRepository _statusRepository;
        private readonly IPriorityRepository _priorityRepository;

        public TaskManagerService(
            ITaskRepository taskRepository,
            ICategoryRepository categoryRepository,
            IStatusRepository statusRepository,
            IPriorityRepository priorityRepository)
        {
            _taskRepository = taskRepository;
            _categoryRepository = categoryRepository;
            _statusRepository = statusRepository;
            _priorityRepository = priorityRepository;
        }

        public List<ToDo_with_CSHARP_v2.EntityCore.Models.Task> SortTasksByPriority(IEnumerable<ToDo_with_CSHARP_v2.EntityCore.Models.Task> tasks)
        {
            return tasks
                .OrderByDescending(t => t.Priority?.Level ?? 0)
                .ThenBy(t => t.DueDate ?? DateTime.MaxValue)
                .ThenBy(t => t.Position)
                .ToList();
        }
    }
}