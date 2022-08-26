using CleanUp.Client.Models.Pages;
using CleanUp.WebApi.Sdk.Models;

namespace CleanUp.Client.Models.Order
{
    public class CartMenu : CartItem
    {
        public int MenuId { get; set; }

        //public List<MenuSlot> Slots { get; set; }

        public Menu Menu { get; set; }

        public List<CartMenuProduct> MenuProducts { get; set; }

        public double Price { get; set; }
        
        public double FinalPrice { get; set; }
        public double DiscountAmount { get; set; }
    }

    public class CartMenuProduct
    {
        public int GroupId { get; set; }
        public int ProductId { get; set; }
    }
}
