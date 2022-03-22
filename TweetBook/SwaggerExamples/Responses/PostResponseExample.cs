using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetBook.Contracts.V1.Responses;

namespace TweetBook.SwaggerExamples.Responses
{
    public class PostResponseExample : IExamplesProvider<PostResponse>
    {
        public PostResponse GetExamples()
        {
            return new PostResponse
            {
                Name = "New post"
            };
        }
    }
}
