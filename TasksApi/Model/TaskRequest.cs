using System.ComponentModel.DataAnnotations.Schema;

namespace TasksApi.Model
{
    public class TaskRequest
    {
        public string Name { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime Ts { get; set; }

        //[ForeignKey("User")]
        public int UserId { get; set; }
    }
}
