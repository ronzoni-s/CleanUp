using CleanUp.WebApi.Sdk.Models;

namespace CleanUp.Client.Models.Order
{
    public class CartProductSlot
    {
        private int quantity;
        
        public int Quantity
        {
            get => quantity;
            set
            {
                if (value >= 0 && (MinimumQuantityThreshold == null || value >= MinimumQuantityThreshold) && (MaximumQuantityThreshold == null || value <= MaximumQuantityThreshold))
                {
                    quantity = value;
                }
            }
        }

        public int? MinimumQuantityThreshold { get; set; }
        public int? MaximumQuantityThreshold { get; set; }
    }

    public class CartProduct
    {
        private double? price = null;

        private double? finalPrice = null;

        public List<CartProductSlot> Slots { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public CartProduct(int slotNumber)
        {
            Slots = new List<CartProductSlot>();
            for (int i = 0; i < slotNumber; i++)
            {
                Slots.Add(new CartProductSlot());
            }
        }

        public double Price
        {
            get
            {
                if (price == null)
                    return Product.CheckoutPrice;
                return price.Value;
            }
            set
            {
                price = value;
            }
        }

        public double FinalPrice { 
            get {
                if (finalPrice == null)
                    return Product.FinalPrice;
                return finalPrice.Value;
            }
            set {
                finalPrice = value;
            }
        }
        
        public double DiscountAmount { get; set; }
    }
}
