using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweetBook.Contracts.V1.Requests.Queries
{
    public class GetAllPostsQuery
    {
        public string UserId { get; set; }
    }
}
