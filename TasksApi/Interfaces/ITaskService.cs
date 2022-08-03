using TasksApi.Model;

namespace TasksApi.Interfaces
{
    public interface ITaskService
    {
        Task<GetTaskResponse> GetTasks(int userId);
        Task<SaveTaskResponse> SaveTask(Model.Task task);

        Task<DeleteResponse> DeleteTask(int taskId, int userId);
    }
}
