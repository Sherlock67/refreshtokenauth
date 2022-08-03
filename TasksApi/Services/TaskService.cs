using Microsoft.EntityFrameworkCore;
using TasksApi.Data;
using TasksApi.Interfaces;
using TasksApi.Model;

namespace TasksApi.Services
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext db;
        public TaskService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<DeleteResponse> DeleteTask(int taskId, int userId)
        {
            var task = await db.Tasks.FindAsync(taskId);

            if (task == null)
            {
                return new DeleteResponse
                {
                    Success = false,
                    Error = "Task not found",
                    ErrorCode = "T01"
                };
            }

            if (task.UserId != userId)
            {
                return new DeleteResponse
                {
                    Success = false,
                    Error = "You don't have access to delete this task",
                    ErrorCode = "T02"
                };
            }

            db.Tasks.Remove(task);

            var saveResponse = await db.SaveChangesAsync();

            if (saveResponse >= 0)
            {
                return new DeleteResponse
                {
                    Success = true,
                    TaskId = task.Id
                };
            }

            return new DeleteResponse
            {
                Success = false,
                Error = "Unable to delete task",
                ErrorCode = "T03"
            };


        }

        public async Task<GetTaskResponse> GetTasks(int userId)
        {
            var tasks = await db.Tasks.Where
                         (o => o.UserId == userId).ToListAsync();

            if (tasks.Count == 0)
            {
                return new GetTaskResponse
                {
                    Success = false,
                    Error = "No tasks found for this user",
                    ErrorCode = "T04"
                };
            }

            return new GetTaskResponse { Success = true, Tasks = tasks };
        }

        public async Task<SaveTaskResponse> SaveTask(Model.Task task)
        {
            await db.Tasks.AddAsync(task);

            var saveResponse = await db.SaveChangesAsync();

            if (saveResponse >= 0)
            {
                return new SaveTaskResponse
                {
                    Success = true,
                    Task = task
                };
            }
            return new SaveTaskResponse
            {
                Success = false,
                Error = "Unable to save task",
                ErrorCode = "T05"
            };
            //throw new NotImplementedException();
        }
    }
}
