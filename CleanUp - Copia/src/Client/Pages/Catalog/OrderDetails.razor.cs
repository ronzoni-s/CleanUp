using ErbertPranzi.Application.Features.Orders.Commands;
using ErbertPranzi.Application.Features.Orders.Commands.Complete;
using ErbertPranzi.Application.Features.Orders.Commands.Void;
using ErbertPranzi.Application.Features.Orders.Queries.GetById;
using ErbertPranzi.Application.Features.Products.Queries.GetOrderProductsPaged;
using ErbertPranzi.Application.Requests.Catalog;
using ErbertPranzi.Application.Requests.Identity;
using ErbertPranzi.Client.Infrastructure.Managers.Catalog.Order;
using ErbertPranzi.Client.Infrastructure.Managers.Catalog.Product;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ErbertPranzi.Client.Pages.Catalog
{
    public partial class OrderDetails
    {
        [Parameter] public int Id { get; set; }
        [Parameter] public string Title { get; set; }
        [Parameter] public string Description { get; set; }

        [Inject] private IOrderManager OrderManager { get; set; }
        [Inject] private IProductManager ProductManager { get; set; }
        [Inject] private IDialogService DialogService { get; set; }

        protected bool NextOrderDisabled { get; set; }
        protected bool PreviousOrderDisabled { get; set; }

        private bool _loaded;

        private GetOrderByIdResponse order; 

        private int _orderNumber;
        private string _contactName;
        private string _contactPhoneNumber;
        private DateTime _orderDate;
        private double _orderWeight;
        private DateTime? _completionDateTime;
        private string _customerName;
        private string _customerAddress;
        private bool _completed => _completionDateTime != null;

        // Polibox and bags cards
        private List<Polibox> _poliboxCards;
        private class Polibox
        {
            public int Number { get; set; }
            public bool Value { get; set; }
            public List<BagState> Bags { get; set; }
        }

        private class BagState
        {
            public bool Enabled { get; set; }
            public bool Value { get; set; }
            public int Number { get; set; }
            public BagState(bool enabled, bool value, int number)
            {
                this.Enabled = enabled;
                this.Value = value;
                Number = number;
            }
        }

        // Products table
        private IEnumerable<GetAllPagedOrderProductsResponse> _pagedData;
        private MudTable<GetAllPagedOrderProductsResponse> _table;
        private int _totalItems;
        private int _currentPage;
        private string _searchString = "";

        private List<BreadcrumbItem> _breadcrumbItems;
        private bool processingCompleteOrder;
        private bool processingVoidOrder;

        protected override async Task OnInitializedAsync()
        {
            NextOrderDisabled = false;
            PreviousOrderDisabled = false;

            _loaded = false;
            var result = await OrderManager.GetOrderByIdAsync(Id);
            if (result.Succeeded)
            {
                order = result.Data;
                if (order != null)
                {
                    _orderNumber = order.OrderNumber;
                    _customerName = order.CustomerName;
                    _customerAddress = order.CustomerAddress;
                    _contactName = order.ContactName;
                    _contactPhoneNumber = order.ContactPhoneNumber;
                    _orderDate = order.OrderDate;
                    _orderWeight = order.Weight;
                    _completionDateTime = order.CompletionDateTime;

                    CreatePoliboxSection();
                }
                Title = "Dettaglio ordine";
                Description = $"{_orderDate.ToString("yyyy-MM-dd")}#{_orderNumber} - {_customerName} ({_customerAddress})";

                _breadcrumbItems = new List<BreadcrumbItem>
                {
                    new BreadcrumbItem("Ordini", href: "/catalog/orders"),
                    new BreadcrumbItem(Title, href: null, disabled: true)
                };
            }
            _loaded = true;
        }

        private void CreatePoliboxSection()
        {
            _poliboxCards = new List<Polibox>();

            _poliboxCards = order.OrderPoliboxs.Select(x => new Polibox()
            {
                Number = x.Number,
                Bags = x.Bags
                    .Select((bag, index) => bag.Status switch {
                        ErbertPranzi.Application.Models.BagStatus.Full => new BagState(false, true, index + 1),
                        //ErbertPranzi.Application.Models.OrderPolibox.BagStatus.Order => order.CompletionDateTime.HasValue || order.CancellationDateTime.HasValue
                        //    ? new BagState(false, true)
                        //    : new BagState(true, false),
                        ErbertPranzi.Application.Models.BagStatus.Order => new BagState(true, false, index + 1),
                        ErbertPranzi.Application.Models.BagStatus.Empty => new BagState(false, false, index + 1),

                        _ => throw new InvalidOperationException("BagStatus not recognized")
                    }
                ).ToList()
            }).ToList();
        }

        private async Task<TableData<GetAllPagedOrderProductsResponse>> ServerReload(TableState state)
        {
            if (!string.IsNullOrWhiteSpace(_searchString))
            {
                state.Page = 0;
            }
            await LoadData(state.Page, state.PageSize, state);
            return new TableData<GetAllPagedOrderProductsResponse> { TotalItems = _totalItems, Items = _pagedData };
        }

        private async Task PrintProducts()
        {
            var confirmCompleteOrderDialogResult = await DialogService.ShowMessageBox(
                    "Stampa prodotti",
                    $"Procedendo verrà stampata l'esportazione dei prodotti relativi all'ordine",
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

        private async Task GoToPreviousOrder()
        {
            var result = await OrderManager.GetPreviousOrderId(order.Id);
            if (result.Succeeded)
            {
                _navigationManager.NavigateTo($"catalog/order-details/{result.Data}");

                await OnInitializedAsync();
                StateHasChanged();
            }
            else
            {
                _snackBar.Add($"Hai raggiunto il primo ordine", Severity.Warning);
                PreviousOrderDisabled = true;
            }

        }

        private async Task GoToNextOrder()
        {
            var result = await OrderManager.GetNextOrderId(order.Id);
            if (result.Succeeded)
            {
                _navigationManager.NavigateTo($"catalog/order-details/{result.Data}"); 

                await OnInitializedAsync();
                StateHasChanged();
            }
            else
            {
                _snackBar.Add($"Hai raggiunto l'ultimo ordine", Severity.Warning);
                NextOrderDisabled = true;
            }

        }

        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
            string[] orderings = null;
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                orderings = state.SortDirection != SortDirection.None ? new[] { $"{state.SortLabel} {state.SortDirection}" } : new[] { $"{state.SortLabel}" };
            }

            var request = new GetAllPagedOrderProductsRequest { OrderId = Id, PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, Orderby = orderings };
            var response = await ProductManager.GetOrderProductsAsync(request);
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

        private void OnSearch(string text)
        {
            _searchString = text;
            _table.ReloadServerData();
        }

        private async Task PrintLabel(int orderId, int poliboxIndex)
        {
            var polibox = _poliboxCards[poliboxIndex];

            var confirmCompleteOrderDialogResult = await DialogService.ShowMessageBox(
                    "Stampa etichetta",
                    $"Procedendo verrà stampatato le etichette selezionate per il Polibox {polibox.Number}",
                    yesText: "Procedi",
                    cancelText: "Annulla");

            if (confirmCompleteOrderDialogResult != true)
            {
                return;
            }

            var request = new PrintPoliboxLabelCommand
            {
                OrderId = orderId,
                PoliboxNumber = polibox.Number,
                PrintPolibox = polibox.Value,
                Bags = polibox.Bags.Where(x => x.Enabled && x.Value).Select(x => x.Number).ToList()
            };

            var result = await OrderManager.PrintPoliboxLabelAsync(request);
            if (result.Succeeded)
            {
                _snackBar.Add("Etichette stampata correttamente!", Severity.Success);
            }
            else
            {
                _snackBar.Add($"Si è verificato un errore", Severity.Error);
            }
        }

        private async Task CompleteOrder()
        {
            var parameters = new DialogParameters();
            parameters.Add("UsedBags", order.Bags);

            var dialog = DialogService.Show<GetUsedBags>("Attenzione", parameters, new DialogOptions()
            {
                DisableBackdropClick = true
            });
            var usedBagsResult = await dialog.Result;
            if (usedBagsResult.Cancelled)
            {
                return;
            }

            var confirmCompleteOrderDialogResult = await DialogService.ShowMessageBox(
                    "Completa ordine",
                    "Procedendo verrà confermato l'ordine e verrà stampato lo scontrino",
                    yesText: "Procedi",
                    cancelText: "Annulla");

            if (confirmCompleteOrderDialogResult != true)
            {
                return;
            }
            
            processingCompleteOrder = true;

            var request = new CompleteOrderCommand 
            {
                Id = Id,
                UsedBags = Convert.ToInt32(usedBagsResult.Data)
            };

            var result = await OrderManager.CompleteAsync(request);
            if (result.Succeeded)
            {
                _snackBar.Add("Ordine completato!", Severity.Success);

                await GoToNextOrder();
            }
            else
            {
                _snackBar.Add($"Si è verificato un errore: {result.Messages.First()}", Severity.Error);
            }

            processingCompleteOrder = false;
        }

        private async Task VoidOrder()
        {
            var confirmVoidOrderDialogResult = await DialogService.ShowMessageBox(
                    "Annulla ordine",
                    "Procedendo verrà annullato l'ordine e verrà stampato lo scontrino di storno (se necessario)",
                    yesText: "Procedi",
                    cancelText: "Annulla");

            if (confirmVoidOrderDialogResult != true)
            {
                return;
            }

            processingVoidOrder = true;

            var request = new VoidOrderCommand
            {
                Id = Id
            };

            var result = await OrderManager.VoidAsync(request);
            if (result.Succeeded)
            {
                _snackBar.Add("Ordine annullato!", Severity.Success);

                await OnInitializedAsync();
                StateHasChanged();      // reloads the page
            }
            else
            {
                _snackBar.Add($"Si è verificato un errore: {result.Messages}", Severity.Error);
            }

            processingVoidOrder = false;
        }
    }
}