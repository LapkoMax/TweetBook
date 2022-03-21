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

        [HttpPost(ApiRoutes.Identity.Register)]
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

        [HttpPost(ApiRoutes.Identity.Login)]
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

        [HttpPost(ApiRoutes.Identity.Refresh)]
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
    }
}
