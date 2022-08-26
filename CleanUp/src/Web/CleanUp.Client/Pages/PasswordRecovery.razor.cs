using CleanUp.WebApi.Sdk.Requests.User;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

namespace CleanUp.Client.Pages
{
    public partial class PasswordRecovery
    {

        // State 
        private string loginValidationMessage = "";
        private bool loading = false;
        private bool sent = false;

        // Data
        private PasswordRecoveryModel passwordRecoveryModel = new();

        //Models
        private class PasswordRecoveryModel
        {
            [Required(ErrorMessage = "Indirizzo email richiesto")]
            [EmailAddress(ErrorMessage ="Indirizzo email non valido")]
            public string Email { get; set; } = "";
        }

        protected override async Task OnInitializedAsync()
        {
           
        }

        private async Task Submit()
        {
            loading = true;
            loginValidationMessage = "";
            StateHasChanged();

            var result = await userManager.ForgotPasswordAsync(new ForgotPasswordRequest
            {
                Email = passwordRecoveryModel.Email,
                IsB2C = false
            });
            loading = false;
            if (result.IsSuccess)
            {
                sent = true;
                StateHasChanged();
                return;
            }
            loginValidationMessage = result.StatusCode != System.Net.HttpStatusCode.InternalServerError ? result.Message : "Si è verificato un errore...";
            StateHasChanged();
        }
    }
}
