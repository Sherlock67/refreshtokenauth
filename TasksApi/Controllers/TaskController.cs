using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TasksApi.Interfaces;
using TasksApi.Model;

namespace TasksApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService taskService;

        public TaskController(ITaskService taskService)
        {
            this.taskService = taskService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var getTasksResponse = await taskService.GetTasks(userId);

            if (!getTasksResponse.Success)
            {
                return UnprocessableEntity(getTasksResponse);
            }

            var tasksResponse = getTasksResponse.Tasks.ConvertAll(o =>
            new TaskResponse
            {
                Id = o.Id,
                IsCompleted = o.IsCompleted,
                Name = o.Name,
                Ts = o.Ts
            });

            return Ok(tasksResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Post(TaskRequest taskRequest)
        {
            var task = new Model.Task
            {
                IsCompleted = taskRequest.IsCompleted,
                Ts = taskRequest.Ts,
                Name = taskRequest.Name,
                //UserId = UserId
            };

            var saveTaskResponse = await taskService.SaveTask(task);

            if (!saveTaskResponse.Success)
            {
                return UnprocessableEntity(saveTaskResponse);
            }

            var taskResponse = new TaskResponse
            {
                Id = saveTaskResponse.Task.Id,
                IsCompleted = saveTaskResponse.Task.IsCompleted,
                Name = saveTaskResponse.Task.Name,
                Ts = saveTaskResponse.Task.Ts
            };

            return Ok(taskResponse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id,int userId)
        {
            var deleteTaskResponse = await taskService.DeleteTask(id, userId);
            if (!deleteTaskResponse.Success)
            {
                return UnprocessableEntity(deleteTaskResponse);
            }

            return Ok(deleteTaskResponse.TaskId);
        }
    }
}
