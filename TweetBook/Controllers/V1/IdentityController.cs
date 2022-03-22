using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetBook.Contracts.V1;
using TweetBook.Contracts.V1.Requests;
using TweetBook.Contracts.V1.Responses;
using TweetBook.Services;

namespace TweetBook.Controllers.V1
{
    public class IdentityController : Controller
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        /// <summary>
        /// Registers a new user in the system
        /// </summary>
        /// <response code="200">Registers a new user in the system</response>
        /// <response code="400">Unable to register new user due to some errors</response>
        [HttpPost(ApiRoutes.Identity.Register)]
        [ProducesResponseType(typeof(List<AuthSuccessResponce>), 200)]
        [ProducesResponseType(typeof(List<AuthFailedResponce>), 400)]
        public async Task<IActionResult> Register([FromBody]UserRegistrationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new AuthFailedResponce
                {
                    Errors = ModelState.Values.SelectMany(x => x.Errors.Select(err => err.ErrorMessage))
                });

            var authResponce = await _identityService.RegisterAsync(request.Email, request.Password);

            if (!authResponce.Success)
                return BadRequest(new AuthFailedResponce
                {
                    Errors = authResponce.Errors
                });

            return Ok(new AuthSuccessResponce 
            {
                Token = authResponce.Token,
                RefreshToken = authResponce.RefreshToken
            });
        }

        /// <summary>
        /// Login in the system with existing user
        /// </summary>
        /// <response code="200">Login in the system with existing user</response>
        /// <response code="400">Unable to login due to some errors</response>
        [HttpPost(ApiRoutes.Identity.Login)]
        [ProducesResponseType(typeof(List<AuthSuccessResponce>), 200)]
        [ProducesResponseType(typeof(List<AuthFailedResponce>), 400)]
        public async Task<IActionResult> Login([FromBody]UserLoginRequest request)
        {
            var authResponce = await _identityService.LoginAsync(request.Email, request.Password);

            if (!authResponce.Success)
                return BadRequest(new AuthFailedResponce
                {
                    Errors = authResponce.Errors
                });

            return Ok(new AuthSuccessResponce
            {
                Token = authResponce.Token,
                RefreshToken = authResponce.RefreshToken
            });
        }

        /// <summary>
        /// Refresh an expired JWT using a refresh token
        /// </summary>
        /// <response code="200">Refresh an expired JWT using a refresh token</response>
        /// <response code="400">Unable to refresh a token due to some errors</response>
        [HttpPost(ApiRoutes.Identity.Refresh)]
        [ProducesResponseType(typeof(List<AuthSuccessResponce>), 200)]
        [ProducesResponseType(typeof(List<AuthFailedResponce>), 400)]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            var authResponce = await _identityService.RefreshTokenAsync(request.Token, request.RefreshToken);

            if (!authResponce.Success)
                return BadRequest(new AuthFailedResponce
                {
                    Errors = authResponce.Errors
                });

            return Ok(new AuthSuccessResponce
            {
                Token = authResponce.Token,
                RefreshToken = authResponce.RefreshToken
            });
        }

        /// <summary>
        /// Add a role to the existing user
        /// </summary>
        /// <response code="200">Add a role to the existing user</response>
        /// <response code="400">Unable to add a role to the user due to some errors</response>
        [HttpPost(ApiRoutes.Identity.AddRole)]
        [ProducesResponseType(typeof(List<ErrorModel>), 400)]
        public async Task<IActionResult> AddRoleToUser([FromBody]AddRoleToUserRequest request)
        {
            var addedRole = await _identityService.AddRoleToUser(request.UserEmail, request.RoleName);

            if (!addedRole)
                return BadRequest(new ErrorModel
                {
                    FieldName = "User id or role name",
                    Message = "Somethisng went wrong, check the spelling of fields"
                });
            return Ok();
        }
    }
}
