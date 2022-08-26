using CurrieTechnologies.Razor.SweetAlert2;
using CleanUp.Client.Authentication;
using CleanUp.Client.Extensions;
using CleanUp.Client.Helpers;
using CleanUp.Client.Managers.Identity.Authentication;
using CleanUp.Client.Managers.Identity.Users;
using CleanUp.Client.Managers.Orders;
using CleanUp.Client.Models.Api;
using CleanUp.Client.Models.Order;
using CleanUp.WebApi.Sdk.Models;
using CleanUp.WebApi.Sdk.Models.AdditionalData;
using System.Text.Json;

namespace CleanUp.Client.Services.User
{
    public class CartService : ICartService
    {
        private readonly ICartManager cartManager;
        private readonly SweetAlertService swal;

        private Cart cart;
        private double? minimumOrderAmount = null;
        private int? minimumOrderItems = null;

        public CartService(ICartManager cartManager, SweetAlertService swal)
        {
            this.cartManager = cartManager;
            this.swal = swal;
            cart = new Cart();
        }

        public double? MinimumOrderAmount
        {
            get => minimumOrderAmount;
            set => minimumOrderAmount = value;
        }
        public int? MinimumOrderItems
        {
            get => minimumOrderItems;
            set => minimumOrderItems = value;
        }
        /// <summary>
        /// FinalPrice without additional amounts
        /// </summary>

        public void SetProducts(List<CartProduct> cartProducts)
        {
            cart.CartProducts = cartProducts;
        }

        public int GetItemsNumber => cart.ItemNumber;
        public List<CartProduct> GetCartProducts() => cart.CartProducts;

        /// <summary>
        /// Prices calculated from product final prices and not from price order api. It does contain delivery amount and coupon discount
        /// </summary>
        public double CalculatedPrice
        {
            get
            {
                double productsAmount = cart.CartProducts.Sum(x => x.FinalPrice * x.Slots.Sum(x => x.Quantity));
                double deliveryAmount = cart.CartAddress?.DeliveryAmount ?? 0;
                if (cart.CartCoupon == null)
                    return productsAmount + (deliveryAmount);
                return cart.CartCoupon.CalculateDiscountedAmount(productsAmount) + deliveryAmount;
            }
        }
        

        public void ResetProductQuantitus(List<int> productIds)
        {
            foreach (var cartProduct in cart.CartProducts)
            {
                if (productIds.Contains(cartProduct.ProductId))
                {
                    cartProduct.Slots.ForEach(slot => slot.Quantity = 0);
                }
            }
        }

        public void Clear()
        {
            cart.Clear();
            minimumOrderAmount = null;
            minimumOrderItems = null;
        }

        public CartCoupon CartCoupon 
        { 
            get => cart.CartCoupon; 
            set => cart.CartCoupon = value;
        }

        public void SelectAddress(int id, double deliveryAmount)
        {
            cart.CartAddress = new CartAddress(id, deliveryAmount);
        }

        public CartAddress SelectedAddress => cart.CartAddress;

        public List<CartDeliveryDate> DeliveryDates {
            get => cart.DeliveryDates;
            set { 
                cart.DeliveryDates = value;
            }
        }

        public async Task<ApiResult<PriceOrder>> ValidateCart(int catalogId, bool validateProductProductionDays, DateTime? orderCreationDate = null, int? orderId = null)
        {
            var orders = new List<PriceOrdersOrderRequest>();
            for (int i = 0; i < cart.DeliveryDates.Count; i++)
            {
                var order = new PriceOrdersOrderRequest
                {
                    ValidateProductProductionDaysForDeliveryDate = validateProductProductionDays ? cart.DeliveryDates[i].Date : null,
                    Products = cart.CartProducts.Where(cartProduct => cartProduct.Slots[i].Quantity > 0).Select(cartProduct => new PlaceOrderProductRequest
                    {
                        ProductId = cartProduct.ProductId,
                        Quantity = cartProduct.Slots[i].Quantity
                    }).ToList(),
                    Menus = new List<PlaceOrderMenuRequest>(),
                };
                orders.Add(order);
            }

            var request = new PriceOrdersRequest
            {
                ValidateWithCouponsForDate = orderCreationDate,
                ValidateFromOrderId = orderId,
                AddressId = cart.CartAddress.Id,
                CatalogId = catalogId,
                Orders = orders
            };
            var response = await cartManager.PriceOrders(request);
            await HandleValidationResponse(response);
            return response;
        }

        public async Task<ApiResult<PriceOrder>> PlaceOrder(int catalogId)
        {
            var orders = new List<PlaceOrdersOrderRequest>();
            for (int i = 0; i < cart.DeliveryDates.Count; i++)
            {
                var order = new PlaceOrdersOrderRequest
                {
                    DeliveryDateTime = cart.DeliveryDates[i].Date,
                    Products = cart.CartProducts.Where(cartProduct => cartProduct.Slots[i].Quantity > 0).Select(cartProduct => new PlaceOrderProductRequest
                    {
                        ProductId = cartProduct.ProductId,
                        Quantity = cartProduct.Slots[i].Quantity
                    }).ToList(),
                    Menus = new List<PlaceOrderMenuRequest>(),
                };
                orders.Add(order);
            }

            var request = new PlaceOrdersRequest
            {
                CatalogId = catalogId,
                AddressId = cart.CartAddress.Id,
                Orders = orders,
                Payments = new List<PlaceOrderPaymentRequest>(),
            };
            var response = await cartManager.PlaceOrders(request);
            await HandleValidationResponse(response);
            return response;
        }

        public async Task<ApiResult<PriceOrder>> EditOrder(int catalogId, int orderId)
        {
            var orders = new List<EditOrdersOrderRequest>();
            for (int i = 0; i < cart.DeliveryDates.Count; i++)
            {
                var order = new EditOrdersOrderRequest
                {
                    OrderId = cart.DeliveryDates[i].OrderId.Value,
                    Products = cart.CartProducts.Where(cartProduct => cartProduct.Slots[i].Quantity > 0).Select(cartProduct => new EditOrderProductRequest
                    {
                        ProductId = cartProduct.ProductId,
                        Quantity = cartProduct.Slots[i].Quantity
                    }).ToList(),
                    Menus = new List<EditOrderMenuRequest>(),
                };
                orders.Add(order);
            }

            var request = new EditOrdersRequest
            {
                CatalogId = catalogId,
                Orders = orders,
                Payments = new List<EditOrderPaymentRequest>(),
            };
            var response = await cartManager.EditOrders(orderId, request);
            await HandleValidationResponse(response);
            return response;
        }

        private async Task HandleValidationResponse(ApiResult<PriceOrder> response)
        {
            this.minimumOrderAmount = null;
            this.minimumOrderItems = null;

            if (response.IsSuccess)
            {
                //this.price = response.Response.BasePrice - response.Response.CartDiscountAmount - response.Response.ItemsDiscountAmount >= 0 ? response.Response.BasePrice - response.Response.CartDiscountAmount - response.Response.ItemsDiscountAmount : 0;
                return;
            }

            if (response.AdditionalData == null)
            {
                await swal.FireAsync("Si è verificato un errore", response.StatusCode != System.Net.HttpStatusCode.InternalServerError ? response.Message : string.Empty, SweetAlertIcon.Error);
            }
            else
            {
                response.AdditionalData.TryGetValue("entity", out var entity);
                response.AdditionalData.TryGetValue("error", out var error);
                if (entity.ToString() == "Cart" && error.ToString() == "Invalid")
                {
                    var productsNotValid = ResultExtensions.Convert<List<ProductNotValid>>((JsonElement)response.AdditionalData["productNotValids"]);
                    await swal.FireAsync(new SweetAlertOptions
                    {
                        Icon = SweetAlertIcon.Error,
                        Title = "Prodotti non validi",
                        Html = $"I seguenti articoli non sono più disponibili: <ul><li>{string.Join("</li><li>", productsNotValid.Select(x => x.Product.Name))}</li></ul> <br/>Proseguendo verranno rimossi"
                    });
                    this.ResetProductQuantitus(productsNotValid.Select(x => x.ProductId).ToList());
                }
                else if (entity.ToString() == "Cart" && error.ToString() == "TotalAmountExceeded")
                {
                    double maximumAmount = ResultExtensions.Convert<double>((JsonElement)response.AdditionalData["maximumAmount"]);
                    await swal.FireAsync("Attenzione", $"E' stato superato l'importo massimo di € {Math.Ceiling(maximumAmount * 100) / 100}. Rimuovere degli articoli per continuare.");
                }
                else if (entity.ToString() == "Cart" && error.ToString() == "MinimumRequirements")
                {
                    this.minimumOrderAmount = ResultExtensions.Convert<double?>((JsonElement)response.AdditionalData["minimumOrderAmount"]);
                    this.minimumOrderItems = ResultExtensions.Convert<int?>((JsonElement)response.AdditionalData["minimumOrderItems"]);
                    await swal.FireAsync("Attenzione", $"Non sono stati superati i requisiti minimi: € {Math.Ceiling((minimumOrderAmount ?? 0) * 100) / 100} e {minimumOrderItems ?? 0} articoli. Aggiungere degli articoli per continuare.");
                }
            }
        }
    }
}
