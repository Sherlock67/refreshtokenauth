using TasksApi.Model;

namespace TasksApi.Interfaces
{
    public interface IUserService
    {

        Task<TokenResponse> LoginAsync(LoginRequest loginRequest);
        Task<SignUpResponse> SignupAsync(SignUpRequest signupRequest);
        Task<LogoutResponse> LogoutAsync(int userId);
    }
}
