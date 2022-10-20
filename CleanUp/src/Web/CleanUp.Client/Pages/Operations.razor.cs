using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;
using CleanUp.WebApi.Sdk.Constants.Permission;
using CleanUp.WebApi.Sdk.Models.Events;
using CleanUp.Client.Managers;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Headers;
using CleanUp.Client.Extensions;
using CleanUp.WebApi.Sdk.Models;
using Microsoft.AspNetCore.Components;

namespace CleanUp.Client.Pages
{
    public partial class Operations
    {
        [Inject] IJSRuntime JS { get; set; }

        private List<CleaningOperation> cleaningOperationList = new();
        private Event _user = new();

        private ClaimsPrincipal _currentUser;
        private bool _canUploadEvents;
        private bool _loaded = false;
        bool alreadyLoadedJs = false;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await authenticationManager.CurrentUser();
            _canUploadEvents = true;

            await GetCleaningOperationsAsync();
            StateHasChanged();
        }

        //protected override async Task OnAfterRenderAsync(bool firstRender)
        //{
        //    if (firstRender)
        //    {
        //        await JS.InvokeVoidAsync("setupCalendarTimeline");
        //    }
        //}

        private async Task GetCleaningOperationsAsync()
        {
            var response = await schedulerManager.GetAllAsync(DateTime.Now.StartOfWeek(DayOfWeek.Monday), DateTime.Now.EndOfWeek(DayOfWeek.Monday));
            if (response.IsSuccess)
            {
                cleaningOperationList = response.Response.ToList();
                _loaded = true;
                StateHasChanged();
                if (!alreadyLoadedJs)
                {
                    await JS.InvokeVoidAsync("setupCalendarTimeline");
                    alreadyLoadedJs = true;
                }
            }
            else
            {
                snackBar.Add("Si è verificato un errore...", Severity.Error);
            }
        }
    }
}