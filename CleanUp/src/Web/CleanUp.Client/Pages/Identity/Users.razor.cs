using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;
using CleanUp.WebApi.Sdk.Models.User;
using CleanUp.WebApi.Sdk.Constants.Permission;

namespace CleanUp.Client.Pages.Identity
{
    public partial class Users
    {
        private List<User> _userList = new();
        private User _user = new();
        private string _searchString = "";

        private ClaimsPrincipal _currentUser;
        private bool _canCreateUsers;
        private bool _canSearchUsers;
        private bool _canViewRoles;
        private bool _loaded = false;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await authenticationManager.CurrentUser();
            _canCreateUsers = (await authorizationService.AuthorizeAsync(_currentUser, Permissions.User.Manage)).Succeeded;
            _canSearchUsers = (await authorizationService.AuthorizeAsync(_currentUser, Permissions.User.View)).Succeeded;
            _canViewRoles = true;

            await GetUsersAsync();
            _loaded = true;
        }

        private async Task GetUsersAsync()
        {
            var response = await userManager.GetAllAsync();
            if (response.IsSuccess)
            {
                _userList = response.Response.ToList();
            }
            else
            {
                //foreach (var message in response.Messages)
                //{
                //    snackBar.Add(message, Severity.Error);
                //}
                snackBar.Add("Si è verificato un errore...", Severity.Error);
            }
        }

        private bool Search(User user)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (user.FirstName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (user.LastName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (user.Email?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (user.PhoneNumber?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (user.UserName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }

        private async Task InvokeModal()
        {
            var parameters = new DialogParameters();
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = dialogService.Show<RegisterUserModal>("Registra nuovo utente", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await GetUsersAsync();
            }
        }

        private void ViewProfile(string userId)
        {
            navigationManager.NavigateTo($"/users/{userId}");
        }

        private void ManageRoles(string userId, string email)
        {
            if (email == "mukesh@blazorhero.com") snackBar.Add("Non permesso", Severity.Error);
            else navigationManager.NavigateTo($"/identity/user-roles/{userId}");
        }
    }
}