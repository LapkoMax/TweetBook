using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetBook.Contracts.V1;
using TweetBook.Contracts.V1.Requests;
using TweetBook.Contracts.V1.Responses;
using TweetBook.Domain;
using TweetBook.Extensions;
using TweetBook.Services;

namespace TweetBook.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    public class PostsController : Controller
    {
        private IPostService _postService;
        private readonly IMapper _mapper;
        public PostsController(IPostService postService, IMapper mapper)
        {
            _postService = postService;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns all the posts in the system
        /// </summary>
        /// <response code="200">Returns all the posts in the system</response>
        [HttpGet(ApiRoutes.Posts.GetAll)]
        [ProducesResponseType(typeof(List<PostResponse>), 200)]
        public async Task<IActionResult> GetAllAsync()
        {
            var posts = await _postService.GetPostsAsync();
            return Ok(_mapper.Map<List<PostResponse>>(posts));
        }

        /// <summary>
        /// Returns the post in the system
        /// </summary>
        /// <response code="200">Returns the post in the system</response>
        /// <response code="404">Post is not found</response>
        [HttpGet(ApiRoutes.Posts.Get)]
        [ProducesResponseType(typeof(PostResponse), 200)]
        public async Task<IActionResult> GetPostAsync([FromRoute]Guid postId)
        {
            var post = await _postService.GetPostByIdAsync(postId);
            if (post == null)
                return NotFound();
            return Ok(_mapper.Map<PostResponse>(post));
        }

        /// <summary>
        /// Creates a post in the system
        /// </summary>
        /// <response code="201">Creates a post in the system</response>
        /// <response code="400">Unable to create a post due to validation error</response>
        [HttpPost(ApiRoutes.Posts.Create)]
        [Authorize(Policy = "MustWorkForChapsas")]
        [ProducesResponseType(typeof(PostResponse), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> CreateAsync([FromBody]CreatePostRequest postRequest)
        {
            var post = new Post
            {
                Name = postRequest.Name,
                UserId = HttpContext.GetUserId()
            };

            var created = await _postService.CreatePostAsync(post);

            if (!created)
                return BadRequest(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Unable to create a post" } } });

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseUrl + "/" + ApiRoutes.Posts.Get.Replace("{postId}", post.Id.ToString());

            return Created(locationUrl, _mapper.Map<PostResponse>(post));
        }

        /// <summary>
        /// Removes a post from the system
        /// </summary>
        /// <response code="204">Removes a post from the system</response>
        /// <response code="400">Unable to remove a post due to authentication error</response>
        /// <response code="404">Unable to remove a post because it is not found</response>
        [HttpDelete(ApiRoutes.Posts.Delete)]
        [Authorize(Policy = "MustWorkForChapsas")]
        [ProducesResponseType(typeof(AuthFailedResponce), 400)]
        public async Task<IActionResult> DeleteAsync([FromRoute]Guid postId)
        {
            var userOwnsPost = await _postService.UserOwnsPostAsync(postId, HttpContext.GetUserId());

            if (!userOwnsPost)
                return BadRequest(new AuthFailedResponce
                {
                    Errors = new List<string> { "Sorry, you don't own this post!" }
                });

            var deleted = await _postService.DeletePostAsync(postId);
            if (!deleted)
                return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Updates a post in the system
        /// </summary>
        /// <response code="200">Updates a post in the system</response>
        /// <response code="400">Unable to update a post due to authentication error</response>
        /// <response code="404">Unable to update a post because it is not found</response>
        [HttpPut(ApiRoutes.Posts.Update)]
        [Authorize(Policy = "MustWorkForChapsas")]
        [ProducesResponseType(typeof(PostResponse), 200)]
        [ProducesResponseType(typeof(AuthFailedResponce), 400)]
        public async Task<IActionResult> UpdateAsync([FromRoute]Guid postId, [FromBody]UpdatePostRequest postRequest)
        {
            var userOwnsPost = await _postService.UserOwnsPostAsync(postId, HttpContext.GetUserId());

            if (!userOwnsPost)
                return BadRequest(new AuthFailedResponce 
                {
                    Errors = new List<string> { "Sorry, you don't own this post!" }
                });

            var post = await _postService.GetPostByIdAsync(postId);
            post.Name = postRequest.Name;

            var updated = await _postService.UpdatePostAsync(post);
            if (!updated)
                return NotFound();
            return Ok(_mapper.Map<PostResponse>(post));
        }
    }
}
