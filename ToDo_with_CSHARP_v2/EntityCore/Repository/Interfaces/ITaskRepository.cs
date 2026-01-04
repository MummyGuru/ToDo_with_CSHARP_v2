using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo_with_CSHARP_v2.EntityCore.Models;

namespace ToDo_with_CSHARP_v2.EntityCore.Repository.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<Models.Task>> GetAllAsync();
        Task<Models.Task> GetByIdAsync(int id);
        Task<int> CreateAsync(Models.Task task);
        Task<bool> UpdateAsync(Models.Task task);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateStatusAsync(int taskId, int statusId);
        Task<IEnumerable<Models.Task>> SearchAsync(string keyword);
        Task<IEnumerable<Models.Task>> GetDueTodayAsync();
        Task<IEnumerable<Models.Task>> GetOverdueAsync();
        Task<IEnumerable<Models.Task>> GetWithDeadlineWarningAsync();
        Task<bool> UpdateDeadlineWarningsAsync();
    }

    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        Task<Category> GetDefaultAsync();
    }

    public interface IStatusRepository
    {
        Task<IEnumerable<Status>> GetAllAsync();
        Task<Status> GetByIdAsync(int id);
        Task<Status> GetDefaultAsync();
    }

    public interface IPriorityRepository
    {
        Task<IEnumerable<Priority>> GetAllAsync();
        Task<Priority> GetByIdAsync(int id);
        Task<Priority> GetDefaultAsync();
    }

    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetByTaskIdAsync(int taskId);
        Task<int> CreateAsync(Comment comment);
        Task<bool> DeleteAsync(int commentId);
    }
}
