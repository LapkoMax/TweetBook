using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetBook.Contracts.V1.Requests.Queries;
using TweetBook.Contracts.V1.Responses;
using TweetBook.Domain;
using TweetBook.Services;

namespace TweetBook.Helpers
{
    public static class PaginationHelpers
    {
        public static PagedResponse<T> CreatePaginationResponse<T>(IUriService uriService, PaginationFilter paginationFilter, List<T> response)
        {
            var nextPage = paginationFilter.PageNumber >= 1 ?
                uriService.GetAllPostsUri(new PaginationQuery(paginationFilter.PageNumber + 1, paginationFilter.PageSize)).ToString()
                : null;

            var previousPage = paginationFilter.PageNumber - 1 >= 1 ?
                uriService.GetAllPostsUri(new PaginationQuery(paginationFilter.PageNumber - 1, paginationFilter.PageSize)).ToString()
                : null;

            return new PagedResponse<T>(response)
            {
                PageNumber = paginationFilter.PageNumber >= 1 ? paginationFilter.PageNumber : (int?)null,
                PageSize = paginationFilter.PageSize >= 1 ? paginationFilter.PageSize : (int?)null,
                NextPage = response.Any() ? nextPage : null,
                PreviousPage = response.Any() ? previousPage : null
            };
        }
    }
}
