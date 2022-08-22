using CleanUp.Application.Features.Orders.Queries.GetAllPaged;
using CleanUp.Application.Requests.Catalog;
using CleanUp.Client.Extensions;
using CleanUp.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CleanUp.Application.Features.Orders.Commands.AddEdit;
using CleanUp.Client.Infrastructure.Managers.Catalog.Order;
using CleanUp.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using CleanUp.Application.Features.Orders.Commands;

namespace CleanUp.Client.Pages.Catalog
{
    public partial class Orders
    {
        [Inject] private IOrderManager OrderManager { get; set; }
        [Inject] private IDialogService DialogService { get; set; }

        //[CascadingParameter] private HubConnection HubConnection { get; set; }



        private IEnumerable<GetAllPagedOrdersResponse> _pagedData;
        private MudTable<GetAllPagedOrdersResponse> _table;
        private int _totalItems;
        private int _currentPage;
        private string _searchString = "";
        private bool _hideCompletedOrders = true;
        private bool hideVoidedOrders = true;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateOrders;
        private bool _canEditOrders;
        private bool _canDeleteOrders;
        private bool _canExportOrders;
        private bool _canSearchOrders;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateOrders = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Orders.Create)).Succeeded;
            _canEditOrders = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Orders.Edit)).Succeeded;
            _canDeleteOrders = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Orders.Delete)).Succeeded;
            _canExportOrders = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Orders.Export)).Succeeded;
            _canSearchOrders = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Orders.Search)).Succeeded;

            _loaded = true;
        }

        private async Task<TableData<GetAllPagedOrdersResponse>> ServerReload(TableState state)
        {
            if (!string.IsNullOrWhiteSpace(_searchString))
            {
                state.Page = 0;
            }
            await LoadData(state.Page, state.PageSize, state);
            return new TableData<GetAllPagedOrdersResponse> { TotalItems = _totalItems, Items = _pagedData };
        }

        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
            string[] orderings = null;
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                orderings = state.SortDirection != SortDirection.None ? new[] {$"{state.SortLabel} {state.SortDirection}"} : new[] {$"{state.SortLabel}"};
            }

            var request = new GetAllPagedOrdersRequest { 
                PageSize = pageSize, 
                PageNumber = pageNumber + 1, 
                SearchString = _searchString,
                Orderby = orderings,
                HideCompleted = _hideCompletedOrders,
                HideVoided = hideVoidedOrders
            };
            var response = await OrderManager.GetOrdersAsync(request);
            if (response.Succeeded)
            {
                _totalItems = response.TotalCount;
                _currentPage = response.CurrentPage;
                _pagedData = response.Data;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private void ToggleCompletedOrders(bool hide)
        {
            _hideCompletedOrders = hide;
            _table.ReloadServerData();
        }
        private void ToggleVoidedOrders(bool hide)
        {
            hideVoidedOrders = hide;
            _table.ReloadServerData();
        }

        private void OnSearch(string text)
        {
            _searchString = text;
            _table.ReloadServerData();
        }

        private async Task PrintProducts()
        {
            var confirmCompleteOrderDialogResult = await DialogService.ShowMessageBox(
                    "Stampa prodotti",
                    $"Procedendo verrà stampata l'esportazione dei prodotti relativi agli ordini selezionati",
                    yesText: "Procedi",
                    cancelText: "Annulla");

            if (confirmCompleteOrderDialogResult != true)
            {
                return;
            }

            var result = await OrderManager.PrintProductsAsync();
            if (result.Succeeded)
            {
                _snackBar.Add("Esportazione stampata correttamente!", Severity.Success);
            }
            else
            {
                _snackBar.Add($"Si è verificato un errore", Severity.Error);
            }

        }

        public void GoToOrderDetails(int id)
        {
            _navigationManager.NavigateTo($"catalog/order-details/{id}");
        }

        public void RowClicked(TableRowClickEventArgs<GetAllPagedOrdersResponse> p)
        {
            _navigationManager.NavigateTo($"catalog/order-details/{p.Item.Id}");
        }
        public void SelectedItemsAreChanged(HashSet<GetAllPagedOrdersResponse> p)
        {
            var ciccio = "1";
        }
    }
}