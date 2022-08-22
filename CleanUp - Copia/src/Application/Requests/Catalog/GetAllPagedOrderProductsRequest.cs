namespace ErbertPranzi.Application.Requests.Catalog
{
    public class GetAllPagedOrderProductsRequest : PagedRequest
    {
        public string SearchString { get; set; }
        public int? OrderId { get; set; }
    }
}