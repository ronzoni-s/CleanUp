namespace CleanUp.Application.Features.Products.Queries.GetAllPaged
{
    public class GetAllPagedProductsResponse
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Tax { get; set; }
        public double Price { get; set; }
        public double Weight { get; set; }
        public bool IsActive { get; set; }
    }
}