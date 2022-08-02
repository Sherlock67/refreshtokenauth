namespace TasksApi.Model
{
    public class GetTaskResponse : BaseResponse
    {
        public List<Task> Tasks { get; set; }
    }
}
