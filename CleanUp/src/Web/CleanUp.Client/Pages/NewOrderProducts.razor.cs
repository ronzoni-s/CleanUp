using CurrieTechnologies.Razor.SweetAlert2;
using CleanUp.Client.Extensions;
using CleanUp.Client.Helpers;
using CleanUp.Client.Models.Api;
using CleanUp.Client.Models.Order;
using CleanUp.Client.Shared.Components;
using CleanUp.WebApi.Sdk.Models;
using CleanUp.WebApi.Sdk.Models.AdditionalData;
using CleanUp.WebApi.Sdk.Requests.User;
using fbognini.Core.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json;

namespace CleanUp.Client.Pages
{
    public partial class NewOrderProducts
    {
        [Parameter] public int? OrderId { get; set; }
        private ProductInfo ProductInfoModal { get; set; }

        // State
        private bool loading = false;
        private bool orderValidationLoading = false;

        // Data
        private int catalogId;
        private DateTime? orderCreationDate = null;
        private List<OrderDeliveryDate> slots;

        //Models
        private enum ACTION
        {
            NEW_ORDER,
            EDIT_ORDER
        };

        private ACTION Action => OrderId == null ? ACTION.NEW_ORDER : ACTION.EDIT_ORDER;

        private bool SendButtonEnabled => cartService.GetItemsNumber > 0 && cartService.SelectedAddress != null && (Action != ACTION.NEW_ORDER || cartService.DeliveryDates.Count != 0);
        private bool MinimumOrderNotReachedButton => cartService.MinimumOrderAmount != null && cartService.CalculatedPrice < cartService.MinimumOrderAmount.Value || cartService.MinimumOrderItems != null && cartService.GetItemsNumber < cartService.MinimumOrderItems.Value;

        protected override async Task OnInitializedAsync()
        {
            loading = true;
            StateHasChanged();

            if (cartService.SelectedAddress == null)
            {
                if (Action == ACTION.NEW_ORDER)
                {
                    navigationManager.NavigateTo("/order/new/schedule");
                }
                else
                {
                    navigationManager.NavigateTo($"/order/edit/{OrderId}/schedule");
                }
                return;
            }

            var catalog = await catalogManager.GetCatalog();
            catalogId = catalog.Id;
            cartService.MinimumOrderAmount = catalog.MinimumOrderAmount;
            cartService.MinimumOrderItems = catalog.MinimumOrderItems;

            

            if (Action == ACTION.EDIT_ORDER)
            {
                await FetchOrderProductsAndUpdateCartQuantity();
            }
            else
            {
                await FetchOrderSlots();
                await FetchAndPopulateCartCoupons(DateTime.Now);
                await FetchProductsAndPopulateCart(slots.Count);
            }

            loading = false;
            StateHasChanged();
        }

        private async Task FetchAndPopulateCartCoupons(DateTime date)
        {
            var cartCouponResponse = await couponManager.GetCartCoupons(catalogId, DateTime.Now);
            if (!cartCouponResponse.IsSuccess)
                throw new Exception(cartCouponResponse.Message);
            var firstCoupon = cartCouponResponse.Response.FirstOrDefault();
            if (firstCoupon != null)
                cartService.CartCoupon = new CartCoupon(firstCoupon.Value, firstCoupon.Coupon.DiscountType);
        }

        private async Task FetchProductsAndPopulateCart(int slotNumber, DateTime? couponsDate = null)
        {
            if (couponsDate == null)
                couponsDate = DateTime.Now;

            var response = await productManager.GetAllAsync(catalogId, couponsDate.Value);
            if (!response.IsSuccess)
            {
                throw new Exception(response.Message);
            }

           
            var cartProducts = response.Response.Select(x => new CartProduct(slotNumber)
            {
                ProductId = x.Id,
                Product = x,
            }).ToList();

            cartService.SetProducts(cartProducts);
        }

        private async Task FetchOrderProductsAndUpdateCartQuantity()
        {
            var response = await orderManager.GetAsync(this.OrderId.Value, catalogId);
            if (!response.IsSuccess)
            {
                throw new Exception(response.Message);
            }
            var order = response.Response;
            orderCreationDate = order.Created;

            if (!OrderHelper.CanAnyChildOrderBeModified(order))
            {
                navigationManager.NavigateTo($"/order/{order.Id}");
                return;
            }

            if (order.OrderCoupons.Any())
                cartService.CartCoupon = new CartCoupon(order.OrderCoupons.First().Value, order.OrderCoupons.First().Coupon.DiscountType);

            cartService.DeliveryDates = order.ChildrenOrders.Select(o => new CartDeliveryDate
            {
                OrderId = o.Id,
                Date = o.OrderDate,
                OriginalProductNumber = o.OrderProducts.Sum(x => x.Quantity)
            }).ToList();
            //await FetchProductsAndPopulateCart(order.ChildrenOrders.Count, response.Response.Created);

            var productsResponse = await productManager.GetAllAsync(catalogId, response.Response.Created);
            if (!productsResponse.IsSuccess)
            {
                throw new Exception(productsResponse.Message);
            }

            // Populating available products with order products + products from api
            var cartProductsToAdd = new List<CartProduct>();
            foreach (var orderProduct in order.OrderProducts)
            {
                if (cartProductsToAdd.Any(x => x.ProductId == orderProduct.ProductId))
                    continue;
                cartProductsToAdd.Add(new CartProduct(cartService.DeliveryDates.Count)
                {
                    ProductId = orderProduct.ProductId,
                    Product = orderProduct.Product,
                    FinalPrice = orderProduct.FinalPrice,
                    Price = orderProduct.Price,
                });
            }
            foreach (var product in productsResponse.Response)
            {
                if (cartProductsToAdd.Any(x => x.ProductId == product.Id))
                    continue;
                cartProductsToAdd.Add(new CartProduct(cartService.DeliveryDates.Count)
                {
                    ProductId = product.Id,
                    Product = product,
                });
            }
            cartService.SetProducts(cartProductsToAdd);

            order.ChildrenOrders = order.ChildrenOrders.OrderBy(x => x.OrderDate).ToList();
            for (int i = 0; i < order.ChildrenOrders.Count; i++)
            {
                bool thisChildOrderCanBeModifiedLight = OrderHelper.CanChildOrderBeModifiedLight(order.ChildrenOrders[i]);
                bool thisChildOrderCanBeModifiedFull = OrderHelper.CanChildOrderBeModifiedFull(order.ChildrenOrders[i]);
                foreach (var childOrderProduct in order.ChildrenOrders[i].OrderProducts)
                {
                    var cartProduct = cartService.GetCartProducts().FirstOrDefault(x => x.ProductId == childOrderProduct.ProductId);
                    if (cartProduct == null)
                        continue;
                    cartProduct.Slots[i].Quantity = childOrderProduct.Quantity;
                    if (thisChildOrderCanBeModifiedLight && childOrderProduct.Product.ProductCatalog != null)
                    {
                        int threshold = childOrderProduct.OriginalQuantity - Math.Min(childOrderProduct.Product.ProductCatalog.RemovableProductNumber, (int)Math.Round(childOrderProduct.OriginalQuantity * childOrderProduct.Product.ProductCatalog.RemovableProductPercentage / 100.0));
                        cartProduct.Slots[i].MinimumQuantityThreshold = threshold > 0 ? threshold : 0;
                        cartProduct.Slots[i].MaximumQuantityThreshold = childOrderProduct.Quantity;
                    }
                    else if (thisChildOrderCanBeModifiedFull)
                    {
                        cartProduct.Slots[i].MinimumQuantityThreshold = null;
                        cartProduct.Slots[i].MaximumQuantityThreshold = null;
                    }
                    else
                    {
                        cartProduct.Slots[i].MinimumQuantityThreshold = cartProduct.Slots[i].Quantity;
                        cartProduct.Slots[i].MaximumQuantityThreshold = cartProduct.Slots[i].Quantity;
                    }
                }
                
                if (!thisChildOrderCanBeModifiedFull)
                {
                    foreach (var cartProduct in cartService.GetCartProducts().Where(x => !order.ChildrenOrders[i].OrderProducts.Any(op => op.ProductId == x.ProductId)))
                    {
                        cartProduct.Slots[i].MinimumQuantityThreshold = 0;
                        cartProduct.Slots[i].MaximumQuantityThreshold = 0;
                        cartProduct.Slots[i].Quantity = 0;
                    }
                }
            }
        }

        private async Task FetchOrderSlots()
        {
            var orderSlotResponse = await catalogManager.GetAvailableOrderSlots();
            if (!orderSlotResponse.IsSuccess)
            {
                throw new Exception(orderSlotResponse.Message);
            }
            slots = orderSlotResponse.Response;
            if (slots.Count == 0)
            {
                navigationManager.NavigateTo("/");
                return;
            }
            cartService.DeliveryDates = slots.Select(x => new CartDeliveryDate
            {
                OrderId = null,
                Date = x.DeliveryDate,
                OriginalProductNumber = 0
            }).ToList();
        }

        private async Task CancelOrder()
        {
            var swalResult = await swal.FireAsync(new SweetAlertOptions
            {
                Title = "Conferma annullamento ordine",
                Html = "Sei sicuro di voler annullare l'ordine in corso?",
                Icon = CurrieTechnologies.Razor.SweetAlert2.SweetAlertIcon.Warning,
                ShowConfirmButton = true,
                DenyButtonText = "Prosegui",
                ShowDenyButton = true,
                ConfirmButtonText = "Torna all'ordine"
            });
            if (!swalResult.IsConfirmed)
            {
                cartService.Clear();
                navigationManager.NavigateTo("/");
                return;
            }
        }

        private async Task<ApiResult<PriceOrder>> ValidateOrder()
        {
            // first of all check if a child order has at least a product and is not emptied
            for (int i = 0; i < cartService.DeliveryDates.Count; i++)
            {
                if (cartService.DeliveryDates[i].OriginalProductNumber > 0 && !cartService.GetCartProducts().Any(x => x.Slots[i].Quantity > 0))
                {
                    await swal.FireAsync("Attenzione", $"Impossibile svuotare l'ordine per il giorno {cartService.DeliveryDates[i].Date.ToString("ddd dd/MM/yyyy", new CultureInfo("it-IT"))} in quanto precedentemente compilato", SweetAlertIcon.Warning);
                    return new ApiResult<PriceOrder>(false, System.Net.HttpStatusCode.BadRequest, null);
                }
            }

            orderValidationLoading = true;
            StateHasChanged();

            var response = await cartService.ValidateCart(this.catalogId, Action == ACTION.NEW_ORDER, orderCreationDate, OrderId);
            
            orderValidationLoading = false;
            StateHasChanged();
            return response;
        }

        private async Task SendOrder()
        {
            var validationResponse = await this.ValidateOrder();
            if (!validationResponse.IsSuccess)
                return;

            var dialogResult = await swal.FireAsync(new SweetAlertOptions 
            {
                Icon = SweetAlertIcon.Question,
                Title = "Conferma invio ordine",
                Text = $"Totale ordine con spese di spedizione: € {Math.Round(validationResponse.Response.FinalPrice, 2).ToString("0.00")}",
                ShowConfirmButton = true,
                ConfirmButtonText = "Invia ordine",
                ShowCancelButton = true,
                CancelButtonText = "Ritorna alla modifica"
            });
            if (!dialogResult.IsConfirmed)
                return;

            orderValidationLoading = true;
            StateHasChanged();
            if (Action == ACTION.NEW_ORDER)
            {
                var response = await cartService.PlaceOrder(this.catalogId);
                if (response.IsSuccess)
                {
                    await swal.FireAsync("Ordine inserito con successo!", icon: SweetAlertIcon.Success);
                    cartService.Clear();
                    navigationManager.NavigateTo("/");
                    return;
                }
            }
            else
            {
                var response = await cartService.EditOrder(this.catalogId, this.OrderId.Value);
                if (response.IsSuccess)
                {
                    await swal.FireAsync("Ordine modificato con successo!", icon: SweetAlertIcon.Success);
                    cartService.Clear();
                    navigationManager.NavigateTo("/");
                    return;
                }
            }
            orderValidationLoading = false;
            StateHasChanged();
        }

        private async Task NavigateBack()
        {
            string route;
            if (Action == ACTION.NEW_ORDER)
                route = "order/new/schedule?preserveState=true";
            else
                route = $"order/{this.OrderId.Value}";

            navigationManager.NavigateTo($"{route}");
        }

    }
}
