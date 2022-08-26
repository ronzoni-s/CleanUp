using CleanUp.Client.Helpers;
using CleanUp.Client.Shared.Components;
using CleanUp.WebApi.Sdk.Models;
using CleanUp.WebApi.Sdk.Requests.User;
using fbognini.Core.Utilities;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

namespace CleanUp.Client.Pages
{
    public partial class Order
    {
        [Parameter] public int OrderId { get; set; }
        private ProductInfo ProductInfoModal { get; set; }

        // State
        private bool loading = true;

        // Data
        private CleanUp.WebApi.Sdk.Models.Order order;
        private int catalogId;

        //Models
        

        protected override async Task OnInitializedAsync()
        {
            loading = true;
            StateHasChanged();

            catalogId = (await catalogManager.GetCatalog()).Id;
            await FetchOrder();

            loading = false;
            StateHasChanged();
        }

        private async Task FetchOrder()
        {
            var response = await orderManager.GetAsync(OrderId, catalogId);
            if (!response.IsSuccess)
            {
                throw new Exception(response.Message);
            }
            order = response.Response;
        }
    }
}
