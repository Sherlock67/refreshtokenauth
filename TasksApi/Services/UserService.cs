using Microsoft.EntityFrameworkCore;
using TasksApi.Data;
using TasksApi.Helper;
using TasksApi.Interfaces;
using TasksApi.Model;

namespace TasksApi.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext db;
        private readonly ITokenService tokenService; 
        public UserService(ApplicationDbContext db, ITokenService tokenService)
        {
            this.db = db;
            this.tokenService = tokenService;
        }

        public async Task<TokenResponse> LoginAsync(LoginRequest loginRequest)
        {
            var user =  db.Users.SingleOrDefault(user => user.Active && user.Email == loginRequest.Email);
            if(user == null)
            {
                return new TokenResponse
                {
                    Success = false,
                    Error = "Email not found",
                    ErrorCode = "L02"
                };
                
            }
            var passwordHash = PasswordHelper.HashUsingPbkdf2(loginRequest.Password,Convert.FromBase64String(user.Password));
           // throw new NotImplementedException();
            if(user.Password == passwordHash)
            {
                return new TokenResponse
                {
                    Success = false,
                    Error = "Invalid Password",
                    ErrorCode = "L03"
                };
            }
            var token = await System.Threading.Tasks.Task.Run(() =>
                tokenService.GenerateTokensAsync(user.Id));
            return new TokenResponse
            {
                Success = true,
                AccessToken = token.Item1,
                RefreshToken = token.Item2
            };
        }

        public async Task<LogoutResponse> LogoutAsync(int userId)
        {
            var refreshToken = await db.RefreshTokens.FirstOrDefaultAsync
                               (o => o.UserId == userId);

            if (refreshToken == null)
            {
                return new LogoutResponse { Success = true };
            }

            db.RefreshTokens.Remove(refreshToken);

            var saveResponse = await db.SaveChangesAsync();

            if (saveResponse >= 0)
            {
                return new LogoutResponse { Success = true };
            }

            return new LogoutResponse
            {
                Success = false,
                Error = "Unable to logout user",
                ErrorCode = "L04"
            };
            //throw new NotImplementedException();
        }

        public async Task<SignUpResponse> SignupAsync(SignUpRequest signupRequest)
        {
            var existingUser = await db.Users.SingleOrDefaultAsync
                           (user => user.Email == signupRequest.Email);

            if (existingUser != null)
            {
                return new SignUpResponse
                {
                    Success = false,
                    Error = "User already exists with the same email",
                    ErrorCode = "S02"
                };
            }

            if (signupRequest.Password != signupRequest.ConfirmPassword)
            {
                return new SignUpResponse
                {
                    Success = false,
                    Error = "Password and confirm password do not match",
                    ErrorCode = "S03"
                };
            }

            if (signupRequest.Password.Length <= 7) 
            {
                return new SignUpResponse
                {
                    Success = false,
                    Error = "Password is weak",
                    ErrorCode = "S04"
                };
            }

            var salt = PasswordHelper.GetSecureSalt();
            var passwordHash = PasswordHelper.HashUsingPbkdf2(signupRequest.Password, salt);

            var user = new User
            {
                Email = signupRequest.Email,
                Password = passwordHash,
                PasswordSalt = Convert.ToBase64String(salt),
                FirstName = signupRequest.FirstName,
                LastName = signupRequest.LastName,
                Ts = signupRequest.Ts,
                Active = true // You can save is false and send confirmation email 
                // to the user, then once the user confirms the email you can make it true
            };

            await db.Users.AddAsync(user);

            var saveResponse = await db.SaveChangesAsync();

            if (saveResponse >= 0)
            {
                return new SignUpResponse { Success = true, Email = user.Email };
            }

            return new SignUpResponse
            {
                Success = false,
                Error = "Unable to save the user",
                ErrorCode = "S05"
            };
        }
    }
}
   