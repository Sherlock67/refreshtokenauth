using System.Text.Json.Serialization;

namespace TasksApi.Model
{
    public class DeleteResponse :BaseResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int TaskId { get; set; }

    }
}
