using Dapper;
using System.Collections.Generic;
using System.Linq;

    public class TaskRepository
    {
        public List<TaskItem> GetAllTasks()
        {
            using (var conn = DbHelper.GetConnection())
            {
                string sql = @"
                    SELECT t.Id, t.Title, t.Description, t.Priority, t.CategoryId, t.StatusId, t.Deadline, t.CreatedAt, t.UpdatedAt,
                           s.Name as StatusName, c.Name as CategoryName, c.ColorCode as CategoryColor
                    FROM Tasks t
                    LEFT JOIN Categories c ON t.CategoryId = c.Id
                    JOIN Statuses s ON t.StatusId = s.Id
                    ORDER BY t.Priority DESC, t.Deadline ASC";

                return conn.Query<TaskItem>(sql).ToList();
            }
        }

        public List<TaskItem> SearchTasks(string keyword)
        {
            using (var conn = DbHelper.GetConnection())
            {
                string sql = @"
                    SELECT t.Id, t.Title, t.Description, t.Priority, t.CategoryId, t.StatusId, t.Deadline, t.CreatedAt, t.UpdatedAt,
                           s.Name as StatusName, c.Name as CategoryName, c.ColorCode as CategoryColor
                    FROM Tasks t
                    LEFT JOIN Categories c ON t.CategoryId = c.Id
                    JOIN Statuses s ON t.StatusId = s.Id
                    WHERE t.Title LIKE @Keyword OR t.Description LIKE @Keyword
                    ORDER BY t.Priority DESC";

                return conn.Query<TaskItem>(sql, new { Keyword = "%" + keyword + "%" }).ToList();
            }
        }

        public void DeleteTask(int id)
        {
            using (var conn = DbHelper.GetConnection())
            {
                conn.Execute("DELETE FROM Tasks WHERE Id = @Id", new { Id = id });
            }
        }

        public void UpdateStatus(int taskId, int newStatusId)
        {
            using (var conn = DbHelper.GetConnection())
            {
                conn.Execute("UPDATE Tasks SET StatusId = @StatusId WHERE Id = @Id",
                    new { Id = taskId, StatusId = newStatusId });
            }
        }

        // Метод для добавления (упрощенный)
        public void AddTask(TaskItem task)
        {
            using (var conn = DbHelper.GetConnection())
            {
                string sql = @"INSERT INTO Tasks (Title, Description, Priority, CategoryId, StatusId, Deadline) 
                               VALUES (@Title, @Description, @Priority, @CategoryId, @StatusId, @Deadline)";
                conn.Execute(sql, task);
            }
        }
    }