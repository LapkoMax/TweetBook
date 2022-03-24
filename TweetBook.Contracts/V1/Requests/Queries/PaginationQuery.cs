using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweetBook.Contracts.V1.Requests.Queries
{
    public class PaginationQuery
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public PaginationQuery()
        {
            PageNumber = 1;
            PageSize = 20;
        }

        public PaginationQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
