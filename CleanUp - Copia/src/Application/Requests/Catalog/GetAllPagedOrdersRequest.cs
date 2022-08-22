namespace ErbertPranzi.Application.Requests.Catalog
{
    public class GetAllPagedOrdersRequest : PagedRequest
    {
        public string SearchString { get; set; }
        public bool HideCompleted { get; set; }
        public bool HideVoided { get; set; }
    }
}