using CurrieTechnologies.Razor.SweetAlert2;
using CleanUp.Client.Extensions;
using CleanUp.Client.Models.Order;
using CleanUp.WebApi.Sdk.Models;
using CleanUp.WebApi.Sdk.Models.AdditionalData;
using CleanUp.WebApi.Sdk.Requests.User;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace CleanUp.Client.Pages
{
    public partial class ScheduleOrder
    {
        [Parameter] public int? OrderId { get; set; }

        // State
        private bool loading = true;
        private bool orderValidationLoading = false;

        // Data
        private int catalogId;
        //private List<OrderDeliveryDate> slots;
        private List<CompanyAddress> companyAddresss;
        private double orderFinalPrice;

        //Models
        private enum ACTION
        {
            NEW_ORDER,
            EDIT_ORDER
        };

        private ACTION Action => OrderId == null ? ACTION.NEW_ORDER : ACTION.EDIT_ORDER;

        protected override async Task OnInitializedAsync()
        {
            loading = true;
            StateHasChanged();


            bool preserveState = false;
            var uri = navigationManager.ToAbsoluteUri(navigationManager.Uri);
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("preserveState", out var _preserveState))
            {
                preserveState = Convert.ToBoolean(_preserveState);
            }

            catalogId = (await catalogManager.GetCatalog()).Id;

            await FetchDeliveryAddresss();
            if (companyAddresss.Count == 0)
            {
                navigationManager.NavigateTo("/");
                return;
            }

            if (!preserveState)
                cartService.Clear();

            if (Action == ACTION.EDIT_ORDER)
            {
                await FetchOrderAndPopulateCartAddress();
                navigationManager.NavigateTo($"/order/edit/{OrderId}/complete");
                return;
            }

            loading = false;
            StateHasChanged();
        }


        private async Task FetchDeliveryAddresss()
        {
            var companyResponse = await companyManager.GetAddresss(catalogId, Action == ACTION.NEW_ORDER);
            if (!companyResponse.IsSuccess)
            {
                throw new Exception(companyResponse.Message);
            }
            companyAddresss = companyResponse.Response;
        }

        private async Task FetchOrderAndPopulateCartAddress()
        {
            var response = await orderManager.GetAsync(this.OrderId.Value, catalogId);
            if (!response.IsSuccess)
            {
                throw new Exception(response.Message);
            }
            cartService.SelectAddress(response.Response.CompanyAddressId, response.Response.AdditionalAmount);
        }

        private async Task SetAddress(int id, double amount)
        {
            cartService.SelectAddress(id, amount);
            StateHasChanged();

            //await ValidateOrder();
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

        private async Task NextPage()
        {
            string route;
            if (Action == ACTION.NEW_ORDER)
                route = "order/new/complete";
            else
                route = $"order/edit/{this.OrderId.Value}/complete";

            navigationManager.NavigateTo(route);
        }

    }
}
