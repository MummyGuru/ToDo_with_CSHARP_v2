using Dapper;
using System.Collections.Generic;
using System.Linq;
using ToDo_with_CSHARP_v2.Models;

namespace ToDo_with_CSHARP_v2.Data
{
    public class TaskRepository
    {
        public List<TaskItem> GetAllTasks()
        {
            using (var conn = DbHelper.GetConnection())
            {
                var sql = @"SELECT t.Id, t.Title, t.Description, t.Priority, t.CategoryId, t.StatusId, t.Deadline, t.CreatedAt, t.UpdatedAt,
                                   s.Name as StatusName, c.Name as CategoryName, c.ColorCode as CategoryColor
                            FROM Tasks t
                            JOIN Statuses s ON t.StatusId = s.Id
                            LEFT JOIN Categories c ON t.CategoryId = c.Id
                            ORDER BY t.Priority DESC, t.Deadline ASC";
                return conn.Query<TaskItem>(sql).ToList();
            }
        }

        public List<TaskItem> SearchTasks(string keyword)
        {
            using (var conn = DbHelper.GetConnection())
            {
                var sql = @"SELECT t.Id, t.Title, t.Description, t.Priority, t.CategoryId, t.StatusId, t.Deadline, t.CreatedAt, t.UpdatedAt,
                                   s.Name as StatusName, c.Name as CategoryName, c.ColorCode as CategoryColor
                            FROM Tasks t
                            JOIN Statuses s ON t.StatusId = s.Id
                            LEFT JOIN Categories c ON t.CategoryId = c.Id
                            WHERE t.Title LIKE @Key OR t.Description LIKE @Key
                            ORDER BY t.Priority DESC";
                return conn.Query<TaskItem>(sql, new { Key = "%" + keyword + "%" }).ToList();
            }
        }

        public void AddTask(TaskItem task)
        {
            using (var conn = DbHelper.GetConnection())
            {
                var sql = @"INSERT INTO Tasks (Title, Description, Priority, CategoryId, StatusId, Deadline) 
                            VALUES (@Title, @Description, @Priority, @CategoryId, @StatusId, @Deadline)";
                conn.Execute(sql, task);
            }
        }

        public void UpdateTask(TaskItem task)
        {
            using (var conn = DbHelper.GetConnection())
            {
                var sql = @"UPDATE Tasks SET Title=@Title, Description=@Description, Priority=@Priority, 
                            CategoryId=@CategoryId, StatusId=@StatusId, Deadline=@Deadline 
                            WHERE Id=@Id";
                conn.Execute(sql, task);
            }
        }

        public void DeleteTask(int id)
        {
            using (var conn = DbHelper.GetConnection())
            {
                conn.Execute("DELETE FROM Tasks WHERE Id = @Id", new { Id = id });
            }
        }

        public List<Status> GetStatuses() =>
            DbHelper.GetConnection().Query<Status>("SELECT * FROM Statuses ORDER BY SortOrder").ToList();

        public List<Category> GetCategories() =>
            DbHelper.GetConnection().Query<Category>("SELECT * FROM Categories").ToList();

        public TaskItem GetTaskById(int id)
        {
            using (var conn = DbHelper.GetConnection())
            {
                var sql = @"SELECT t.Id, t.Title, t.Description, t.Priority, t.CategoryId, t.StatusId, t.Deadline, t.CreatedAt, t.UpdatedAt,
                                   s.Name as StatusName, c.Name as CategoryName, c.ColorCode as CategoryColor
                            FROM Tasks t
                            JOIN Statuses s ON t.StatusId = s.Id
                            LEFT JOIN Categories c ON t.CategoryId = c.Id
                            WHERE t.Id = @Id";
                return conn.QueryFirstOrDefault<TaskItem>(sql, new { Id = id });
            }
        }
    }
}