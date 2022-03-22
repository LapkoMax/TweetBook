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
    public class PostsController : Controller
    {
        private IPostService _postService;
        private readonly IMapper _mapper;
        public PostsController(IPostService postService, IMapper mapper)
        {
            _postService = postService;
            _mapper = mapper;
        }

        [HttpGet(ApiRoutes.Posts.GetAll)]
        public async Task<IActionResult> GetAllAsync()
        {
            var posts = await _postService.GetPostsAsync();
            return Ok(_mapper.Map<List<PostResponse>>(posts));
        }

        [HttpGet(ApiRoutes.Posts.Get)]
        public async Task<IActionResult> GetPostAsync([FromRoute]Guid postId)
        {
            var post = await _postService.GetPostByIdAsync(postId);
            if (post == null)
                return NotFound();
            return Ok(_mapper.Map<PostResponse>(post));
        }

        [HttpPost(ApiRoutes.Posts.Create)]
        [Authorize(Policy = "MustWorkForChapsas")]
        public async Task<IActionResult> CreateAsync([FromBody]CreatePostRequest postRequest)
        {
            var post = new Post
            {
                Name = postRequest.Name,
                UserId = HttpContext.GetUserId()
            };

            await _postService.CreatePostAsync(post);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseUrl + "/" + ApiRoutes.Posts.Get.Replace("{postId}", post.Id.ToString());

            return Created(locationUrl, _mapper.Map<PostResponse>(post));
        }

        [HttpDelete(ApiRoutes.Posts.Delete)]
        [Authorize(Policy = "MustWorkForChapsas")]
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

        [HttpPut(ApiRoutes.Posts.Update)]
        [Authorize(Policy = "MustWorkForChapsas")]
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
