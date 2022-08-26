using CleanUp.Client.Helpers;
using CleanUp.WebApi.Sdk.Models;
using CleanUp.WebApi.Sdk.Requests.User;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

namespace CleanUp.Client.Pages
{
    public partial class Index
    {

        // State
        private const int pageSize = 10;
        private bool loading = true;
        private int pageNumber = 1;
        private bool canCreateNewOrder = false;

        // Data
        private FilterModel filterModel = new();
        private List<CleanUp.WebApi.Sdk.Models.Order> orders;
        private int total = 1;
        private int catalogId;
        private int TotalPages => (int) Math.Ceiling((double) total / pageSize);
        private List<CompanyAddress> companyAddresss;

        //Models
        private class FilterModel
        {
            public DateTime? FromDate { get; set; } = DateTime.Now.AddDays(-7);
            public DateTime? ToDate { get; set; } = DateTime.Now;
            public int? CompanyAddressId {get; set; }
        }

        protected override async Task OnInitializedAsync()
        {
            loading = true;
            StateHasChanged();

            catalogId = (await catalogManager.GetCatalog()).Id;
            await FetchCompanyAddresss();
            await UpdateOrders(false);

            await CheckNewOrderCreation();
            await FetchOrderSlots();

            loading = false;
            StateHasChanged();
        }

        private async Task UpdateOrders(bool handleLoading = true)
        {
            if (handleLoading)
            {
                loading = true;
                StateHasChanged();
            }

            var response = await orderManager.GetAllAsync(pageSize, pageNumber, filterModel.FromDate, filterModel.ToDate, filterModel.CompanyAddressId);
            if (!response.IsSuccess)
            {
                throw new Exception(response.Message);
            }
            orders = response.Response.ToList();

            total = response.Pagination.Total;

            if (handleLoading)
                loading = false;
            
            StateHasChanged();
        }

        private async Task FetchCompanyAddresss()
        {
            var addresssResponse = await companyManager.GetAddresss(catalogId, false);
            if (!addresssResponse.IsSuccess)
            {
                throw new Exception(addresssResponse.Message);
            }
            companyAddresss = addresssResponse.Response;
        }

        private async Task FetchOrderSlots()
        {
            var orderSlotResponse = await catalogManager.GetAvailableOrderSlots();
            if (!orderSlotResponse.IsSuccess)
            {
                throw new Exception(orderSlotResponse.Message);
            }
            if (orderSlotResponse.Response.Count == 0)
            {
                canCreateNewOrder = false;
            }
        }

        private async Task CheckNewOrderCreation()
        {
            var addresssResponse = await companyManager.GetAddresss(catalogId, true);
            if (!addresssResponse.IsSuccess)
            {
                throw new Exception(addresssResponse.Message);
            }
            if (addresssResponse.Response.Count > 0)
                canCreateNewOrder = true;
        }

        private void OnSelectedCompanyAddressChange(ChangeEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Value.ToString()))
                filterModel.CompanyAddressId = null;
            else
                filterModel.CompanyAddressId = Convert.ToInt32(e.Value);
            UpdateOrders();
        }

        private void NextPage()
        {
            if (pageNumber < TotalPages)
            {
                pageNumber++;
                UpdateOrders();
            }
        }

        private void PreviousPage()
        {
            if (pageNumber > 1)
            {
                pageNumber--;
                UpdateOrders();
            }
        }

        private void LastPage()
        {
            if (pageNumber < TotalPages)
            {
                pageNumber = TotalPages;
                UpdateOrders();
            }
        }

        private void FirstPage()
        {
            if (pageNumber > 1)
            {
                pageNumber = 1;
                UpdateOrders();
            }
        }
    }
}
