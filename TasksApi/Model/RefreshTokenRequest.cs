namespace TasksApi.Model
{
    public class RefreshTokenRequest
    {
        public int UserId { get; set; }

        public string ReToken { get; set; }
    }
}
