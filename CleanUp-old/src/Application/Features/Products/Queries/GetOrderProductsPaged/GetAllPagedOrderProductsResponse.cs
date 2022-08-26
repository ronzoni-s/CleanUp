using CleanUp.Application.Features.Products.Queries.GetAllPaged;

namespace CleanUp.Application.Features.Products.Queries.GetOrderProductsPaged
{
    public class GetAllPagedOrderProductsResponse
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int Tax { get; set; }
        public double Price { get; set; }
        public double FinalPrice { get; set; }
        public double ProductWeight { get; set; }
        public int? MenuId { get; set; }

        public GetAllPagedProductsResponse Product { get; set; }
    }
}