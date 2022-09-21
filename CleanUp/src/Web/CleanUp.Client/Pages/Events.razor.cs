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

namespace CleanUp.Client.Pages
{
    public partial class Events
    {
        private List<Event> eventList = new();
        private Event _user = new();
        private string _searchString = "";

        private string fileName = null;

        private ClaimsPrincipal _currentUser;
        private bool _canUploadEvents;
        private bool _loaded = false;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await authenticationManager.CurrentUser();
            //_canUploadEvents = (await authorizationService.AuthorizeAsync(_currentUser, Permissions.Event.Manage)).Succeeded;
            _canUploadEvents = true;

            await GetEventsAsync();
            _loaded = true;
        }

        private async Task GetEventsAsync()
        {
            var response = await eventManager.GetAllAsync();
            if (response.IsSuccess)
            {
                eventList = response.Response.ToList();
            }
            else
            {
                snackBar.Add("Si è verificato un errore...", Severity.Error);
            }
        }

        private bool Search(Event user)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (user.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (user.ClassroomId?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }

        private async Task InvokeModal()
        {
            var parameters = new DialogParameters();
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = dialogService.Show<UploadEventsModal>("Carica file", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await GetEventsAsync();
            }
        }

        private void ViewProfile(int id)
        {
            navigationManager.NavigateTo($"/events/{id}");
        }

        private async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            long maxFileSize = 600000;
            var upload = false;

            using var content = new MultipartFormDataContent();

            var _file = e.File;
            try
            {
                var fileContent =
                    new StreamContent(_file.OpenReadStream(maxFileSize));

                fileContent.Headers.ContentType =
                    new MediaTypeHeaderValue(_file.ContentType);

                fileName = _file.Name;

                content.Add(
                    content: fileContent,
                    name: "\"files\"",
                    fileName: _file.Name);

                upload = true;
            }
            catch (Exception ex)
            {
                snackBar.Add("Si è verificato un errore...", Severity.Error);
            }
            

            if (upload)
            {
                var result = await eventManager.UploadAsync(content);
            }
            
        }
    }
}