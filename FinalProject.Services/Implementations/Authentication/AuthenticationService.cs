using FinalProject.Services.Interfaces.Authentication;
using FinalProject.Repository.Interfaces.User;
using FinalProject.Repository.Helpers;
using FinalProject.Services.Helpers;
using FinalProject.Services.DTOs.Responses.User;
using FinalProject.Services.DTOs.Requests.Authentication.Login;
using FinalProject.Services.DTOs.Responses.Authentication.Login;
using FinalProject.Services.DTOs.Responses.Authentication.Register;
using FinalProject.Services.DTOs.Requests.Authentication.Register;

namespace FinalProject.Services.Implementations.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;

        public AuthenticationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Data.Email) || string.IsNullOrEmpty(request.Data.Password))
            {
                return new LoginResponse()
                {
                    Status = false,
                    Message = "Email and password are required!"
                };
            }

            var queryParameters = new QueryParameters();
            queryParameters.AddWhere("email", request.Data.Email);

            var user = await _userRepository.RetrieveAll(queryParameters).SingleOrDefaultAsync();
            var passwordHash = SecurityHelper.HashPassword(request.Data.Password);

            if (user == null || user.Password != passwordHash)
            {
                return new LoginResponse()
                {
                    Status = false,
                    Message = "Invalid email or password!"
                };
            }

            try
            {
                var userResponse = MapUserToUserResponseDto(user);

                return new LoginResponse()
                {
                    Status = true,
                    Message = "Login successful!",
                    Data = userResponse
                };
            }
            catch (Exception ex)
            {
                return new LoginResponse()
                {
                    Status = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<RegisterResponse> Register(RegisterRequest request)
        {
            if (string.IsNullOrEmpty(request.Data.Email) || string.IsNullOrEmpty(request.Data.Password) || string.IsNullOrEmpty(request.Data.FullName) || string.IsNullOrEmpty(request.Data.ConfirmPassword) || string.IsNullOrEmpty(request.Data.Username))
            {
                return new RegisterResponse()
                {
                    Status = false,
                    Message = "All fields are required!"
                };
            }

            if (request.Data.Password != request.Data.ConfirmPassword)
            {
                return new RegisterResponse()
                {
                    Status = false,
                    Message = "Passwords do not match!"
                };
            }

            if (request.Data.Password.Length < 8)
            {
                return new RegisterResponse()
                {
                    Status = false,
                    Message = "Password must be at least 8 characters long!"
                };
            }

            if (request.Data.Username.Length < 2 || request.Data.Username.Length > 255)
            {
                return new RegisterResponse()
                {
                    Status = false,
                    Message = "Username must be between 2 and 255 characters!"
                };
            }

            if (request.Data.FullName.Length < 3 || request.Data.FullName.Length > 255)
            {
                return new RegisterResponse()
                {
                    Status = false,
                    Message = "Full name must be between 3 and 255 characters!"
                };
            }


            var queryParameters = new QueryParameters();
            queryParameters.AddWhere("email", request.Data.Email);

            var existingUser = await _userRepository.RetrieveAll(queryParameters).SingleOrDefaultAsync();
            if (existingUser != null)
            {
                return new RegisterResponse()
                {
                    Status = false,
                    Message = "User with this email already exists!"
                };
            }

            var user = new Models.User()
            {
                Email = request.Data.Email,
                Username = request.Data.Username,
                Password = SecurityHelper.HashPassword(request.Data.Password),
                FullName = request.Data.FullName,
            };

            try
            {
                var id = await _userRepository.Create(user);

                user.Id = id;

                return new RegisterResponse()
                {
                    Status = true,
                    Message = "User registered successfully!",
                    Data = MapUserToUserResponseDto(user)
                };
            }
            catch (Exception ex)
            {
                return new RegisterResponse()
                {
                    Status = false,
                    Message = ex.Message
                };
            }
        }

        private UserResponseDto MapUserToUserResponseDto(Models.User user)
        {
            return new UserResponseDto()
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                FullName = user.FullName,
            };
        }
    }
}