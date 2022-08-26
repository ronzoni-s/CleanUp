using CleanUp.Client.Managers.Orders;
using CleanUp.WebApi.Sdk.Models;

namespace CleanUp.Client.Models.Order
{
    public class Cart
    {        
        public CartAddress CartAddress { get; set; }

        public List<CartProduct> CartProducts { get; set; }
        public List<CartDeliveryDate> DeliveryDates { get; set; }
        
        public CartCoupon CartCoupon { get; set; }

        public Cart()
        {
            CartProducts = new List<CartProduct>();
            DeliveryDates = new List<CartDeliveryDate>();
        }

        public int ItemNumber
        {
            get => CartProducts.Sum(p => p.Slots.Sum(slot => slot.Quantity));
        }

        public void Clear()
        {
            CartAddress = null;
            CartProducts.Clear();
            DeliveryDates.Clear();
            CartCoupon = null;
        }
    }

    public class CartDeliveryDate
    {
        public int? OrderId { get; set; }
        public DateTime Date { get; set; }
        public int OriginalProductNumber { get; set; }
    }

    public class CartAddress
    {
        public int Id { get; set; }
        public double DeliveryAmount { get; set; }

        public CartAddress(int id, double deliveryAmount)
        {
            this.Id = id;
            this.DeliveryAmount = deliveryAmount;
        }
    }

    public class CartCoupon
    {
        public double Value { get; set;}
        public string DiscountType { get; set; }
        
        public CartCoupon(double value, string discountType)
        {
            this.Value = value;
            this.DiscountType = discountType;
        }

        public double CalculateDiscountedAmount(double discountablePrice)
        {
            switch (this.DiscountType)
            {
                // Price
                case "PRC": return Math.Round(discountablePrice - this.Value > 0 ? discountablePrice - this.Value : 0, 2);
                case "PER": return Math.Round(discountablePrice - discountablePrice * this.Value / 100, 2);
                default: throw new NotImplementedException();
            }
        }
    }
}
