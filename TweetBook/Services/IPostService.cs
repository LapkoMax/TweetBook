using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TweetBook.Domain;

namespace TweetBook.Services
{
    public interface IPostService
    {
        Task<List<Post>> GetPostsAsync(GetAllPostsFilter filter = null, PaginationFilter paginationFilter = null);

        Task<Post> GetPostByIdAsync(Guid postId);

        Task<bool> CreatePostAsync(Post post);

        Task<bool> UpdatePostAsync(Post postToUpdate);

        Task<bool> UserOwnsPostAsync(Guid postId, string userId);

        Task<bool> DeletePostAsync(Guid postId);
    }
}
