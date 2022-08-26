using CleanUp.Client.Models.Api;
using CleanUp.Client.Models.Order;
using CleanUp.WebApi.Sdk.Models;

namespace CleanUp.Client.Services.User
{
    public interface ICartService
    {
        double? MinimumOrderAmount { get; set;  }
        int? MinimumOrderItems { get; set;  }
        int GetItemsNumber { get; }
        double CalculatedPrice { get; }
        CartAddress SelectedAddress { get; }
        List<CartDeliveryDate> DeliveryDates { get; set; }
        CartCoupon CartCoupon { get; set; }

        void Clear();
        Task<ApiResult<PriceOrder>> EditOrder(int catalogId, int orderId);
        List<CartProduct> GetCartProducts();
        Task<ApiResult<PriceOrder>> PlaceOrder(int catalogId);
        void ResetProductQuantitus(List<int> productIds);
        void SelectAddress(int id, double deliveryAmount);
        void SetProducts(List<CartProduct> cartProducts);
        Task<ApiResult<PriceOrder>> ValidateCart(int catalogId, bool validateProductProductionDays, DateTime? orderCreationDate = null, int? orderId = null);
    }
}
