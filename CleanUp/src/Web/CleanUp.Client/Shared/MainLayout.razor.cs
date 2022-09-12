using CleanUp.Client.Extensions;
using MudBlazor;
using System;
using System.Linq;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components;

namespace CleanUp.Client.Shared
{
    public partial class MainLayout : IDisposable
    {

        private string CurrentUserId { get; set; }
        private string ImageDataUrl { get; set; }
        private string FirstName { get; set; }
        private string SecondName { get; set; }
        private string Email { get; set; }
        private char FirstLetterOfName { get; set; }

        private async Task LoadDataAsync()
        {
            var state = await stateProvider.GetAuthenticationStateAsync();
            var user = state.User;
            if (user == null) return;
            if (user.Identity?.IsAuthenticated == true)
            {
                CurrentUserId = user.GetUserId();                
                FirstName = user.GetFirstName();
                if (FirstName.Length > 0)
                {
                    FirstLetterOfName = FirstName[0];
                }
                SecondName = user.GetLastName();
                Email = user.GetEmail();
                
                var currentUserResult = await userManager.GetAsync(CurrentUserId);
                if (!currentUserResult.IsSuccess || currentUserResult.Response == null)
                {
                    snackBar.Add("Non sei più autenticato perché il token è stato eliminato", Severity.Error);
                    await authenticationManager.Logout();
                }

                //await hubConnection.SendAsync(ApplicationConstants.SignalR.OnConnect, CurrentUserId);
            }
        }

        private MudTheme _currentTheme;
        private bool _drawerOpen = true;
        private bool _rightToLeft = false;
        

        protected override async Task OnInitializedAsync()
        {
            //_currentTheme = BlazorHeroTheme.DefaultTheme;
            //_currentTheme = await clientPreferenceManager.GetCurrentThemeAsync();
            //_rightToLeft = await clientPreferenceManager.IsRTL();

            interceptor.RegisterEvent();
        }

        private void Logout()
        {
            var parameters = new DialogParameters
            {
                {nameof(Dialogs.Logout.ContentText), $"Conferma logout"},
                {nameof(Dialogs.Logout.ButtonText), $"Logout"},
                {nameof(Dialogs.Logout.Color), Color.Error},
                {nameof(Dialogs.Logout.CurrentUserId), CurrentUserId},
            };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };

            dialogService.Show<Dialogs.Logout>("Logout", parameters, options);
        }

        public void Dispose()
        {
            interceptor.DisposeEvent();
            //_ = hubConnection.DisposeAsync();
        }
    }
}