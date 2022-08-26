using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.WebApi.Sdk.Models
{
    public class PaginationResponse<TClass>
        where TClass : class
    {
        public PaginationResponse()
        {
            Pagination = new Pagination();
        }
        public List<TClass> Response { get; set; }
        public Pagination Pagination { get; set; }
    }

    public class Pagination
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public int? Total { get; set; }
        public string ContinuationSince { get; set; }
        public int? PartialTotal { get; set; }
    }

    public class PaginationResult
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
    }
}
