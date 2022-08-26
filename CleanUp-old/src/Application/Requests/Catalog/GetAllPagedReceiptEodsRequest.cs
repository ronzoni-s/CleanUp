namespace CleanUp.Application.Requests.Catalog
{
    public class GetAllPagedReceiptEodsRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}