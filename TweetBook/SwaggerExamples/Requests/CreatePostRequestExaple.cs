using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetBook.Contracts.V1.Requests;

namespace TweetBook.SwaggerExamples.Requests
{
    public class CreatePostRequestExaple : IExamplesProvider<CreatePostRequest>
    {
        public CreatePostRequest GetExamples()
        {
            return new CreatePostRequest
            {
                Name = "New post"
            };
        }
    }
}
