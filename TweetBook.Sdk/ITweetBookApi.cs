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
    [Headers("Authorization: bearer")]
    public interface ITweetBookApi
    {
        [Get("/api/v1/posts")]
        Task<ApiResponse<List<PostResponse>>> GetAllAsync();

        [Get("/api/v1/posts/{postId}")]
        Task<ApiResponse<PostResponse>> GetPostAsync(Guid postId);

        [Post("/api/v1/posts")]
        Task<ApiResponse<PostResponse>> CreatePostAsync([Body] CreatePostRequest createPostRequest);

        [Delete("/api/v1/posts/{postId}")]
        Task<ApiResponse<string>> DeletePostAsync(Guid postId);

        [Put("/api/v1/posts/{postId}")]
        Task<ApiResponse<PostResponse>> UpdatePostAsync(Guid postId, [Body] UpdatePostRequest updatePostRequest);
    }
}
