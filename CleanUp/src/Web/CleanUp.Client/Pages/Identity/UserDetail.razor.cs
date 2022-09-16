using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        //private async Task ToggleUserStatus()
        //{
        //    var request = new ToggleUserStatusRequest { ActivateUser = _active, UserId = Id };
        //    var result = await userManager.ToggleUserStatusAsync(request);
        //    if (result.Succeeded)
        //    {
        //        snackBar.Add("Stato utente aggiornato", Severity.Success);
        //        navigationManager.NavigateTo("/identity/users");
        //    }
        //    else
        //    {
        //        foreach (var error in result.Messages)
        //        {
        //            snackBar.Add(error, Severity.Error);
        //        }
        //    }
        //}

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

            _loaded = true;
        }
    }
}