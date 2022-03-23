using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetBook.Contracts.V1.Requests;
using TweetBook.Contracts.V1.Responses;

namespace TweetBook.Sdk
{
    public interface IIdentityApi
    {
        [Post("/api/v1/identity/register")]
        Task<ApiResponse<AuthSuccessResponce>> RegisterAsync([Body] UserRegistrationRequest registrationRequest);

        [Post("/api/v1/identity/login")]
        Task<ApiResponse<AuthSuccessResponce>> LoginAsync([Body] UserLoginRequest loginRequest);

        [Post("/api/v1/identity/refresh")]
        Task<ApiResponse<AuthSuccessResponce>> RegisterAsync([Body] RefreshTokenRequest refreshTokenRequest);

        [Post("/api/v1/identity/addRole")]
        Task<ApiResponse<string>> AddRoleToUserAsync([Body] AddRoleToUserRequest addRoleToUserRequest);
    }
}
