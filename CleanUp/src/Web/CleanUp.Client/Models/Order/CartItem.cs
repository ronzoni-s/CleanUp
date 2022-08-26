using CleanUp.WebApi.Sdk.Models;

namespace CleanUp.Client.Models.Order
{
    public abstract class CartItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
    }
}
