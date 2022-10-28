using CleanUp.Client.Extensions;
using CleanUp.WebApi.Sdk.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CleanUp.Client.Pages.AddWorkDayModal;

namespace CleanUp.Client.Pages.Identity
{
    public partial class UserDetail
    {
        [Parameter] public string Id { get; set; }
        [Parameter] public string Title { get; set; }
        [Parameter] public string Description { get; set; }

        private char _firstLetterOfName;
        private string _firstName;
        private string _lastName;
        private string _phoneNumber;
        private string _email;

        private bool _loaded;

        private List<WorkDay> workDays = new();

        [Parameter] public string ImageDataUrl { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var userId = Id;
            var result = await userManager.GetAsync(userId);
            if (result.IsSuccess)
            {
                var user = result.Response;
                if (user != null)
                {
                    _firstName = user.FirstName;
                    _lastName = user.LastName;
                    _email = user.Email;
                    _phoneNumber = user.PhoneNumber;
                }
                Title = $"{_firstName} {_lastName}";
                Description = _email;
                if (_firstName.Length > 0)
                {
                    _firstLetterOfName = _firstName[0];
                }
            }

            await GetWorkDays();

            _loaded = true;
        }

        private async Task GetWorkDays()
        {
            var result = await userManager.GetWorkDays(Id, DateTime.Now.Date.StartOfWeek(DayOfWeek.Monday), DateTime.Now.Date.EndOfWeek(DayOfWeek.Monday));
            if (!result.IsSuccess)
            {
                snackBar.Add("Errore nell'ottenere gli orari dell'utente", Severity.Error);
                return;
            }
            workDays = result.Response;
        }

        private async Task InvokeCreateWorkDay()
        {
            var parameters = new DialogParameters();
            parameters.Add("Model", new AddWorkDayModel
            {
                UserId = Id,
                Date = DateTime.Now,
            });
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = dialogService.Show<AddWorkDayModal>("Inserisci evento", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await GetWorkDays();
            }
        }

        private async Task DeleteUser()
        {
            var result = await userManager.DeleteAsync(Id);
            if (result.IsSuccess)
            {
                snackBar.Add("Utente eliminato correttamente", Severity.Success);
                navigationManager.NavigateTo("/users");
            }
            else
            {
                snackBar.Add("Errore nell'eliminazione dell'utente", Severity.Error);
            }
        }
    }
}