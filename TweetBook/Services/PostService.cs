using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetBook.Data;
using TweetBook.Domain;

namespace TweetBook.Services
{
    public class PostService : IPostService
    {
        private readonly DataContext _dataContext;

        public PostService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<Post>> GetPostsAsync(GetAllPostsFilter filter = null, PaginationFilter paginationFilter = null)
        {
            var querable = _dataContext.Posts.AsQueryable();

            if (paginationFilter == null) 
                return await querable.ToListAsync();

            querable = AddFiltersOnQuery(filter, querable);

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;

            return await querable.Skip(skip).Take(paginationFilter.PageSize).OrderBy(x => x.Id).ToListAsync();
        }

        public async Task<Post> GetPostByIdAsync(Guid postId)
        {
            return await _dataContext.Posts.SingleOrDefaultAsync(x => x.Id == postId);
        }

        public async Task<bool> CreatePostAsync(Post post)
        {
            await _dataContext.Posts.AddAsync(post);
            var created = await _dataContext.SaveChangesAsync();
            return created > 0;
        }

        public async Task<bool> UpdatePostAsync(Post postToUpdate)
        {
            _dataContext.Posts.Update(postToUpdate);
            var updated = await _dataContext.SaveChangesAsync();
            return updated > 0;

        }

        public async Task<bool> UserOwnsPostAsync(Guid postId, string userId)
        {
            var existingPost = await GetPostByIdAsync(postId);
            if (existingPost != null && existingPost.UserId == userId)
                return true;
            return false;
        }

        public async Task<bool> DeletePostAsync(Guid postId)
        {
            var post = await GetPostByIdAsync(postId);
            if (post == null)
                return false;
            _dataContext.Posts.Remove(post);
            var deleted = await _dataContext.SaveChangesAsync();
            return deleted > 0;
        }

        private static IQueryable<Post> AddFiltersOnQuery(GetAllPostsFilter filter, IQueryable<Post> posts)
        {
            if (!string.IsNullOrEmpty(filter?.UserId))
                posts = posts.Where(x => x.UserId == filter.UserId);
            return posts;
        }
    }
}
