namespace ErbertPranzi.Application.Requests.Catalog
{
    public class GetAllPagedProductsRequest : PagedRequest
    {
        public string SearchString { get; set; }
        public bool HideNotActive { get; set; }
    }
}