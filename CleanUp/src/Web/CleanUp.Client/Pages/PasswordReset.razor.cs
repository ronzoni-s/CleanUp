using CleanUp.WebApi.Sdk.Requests.User;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;

namespace CleanUp.Client.Pages
{
    public partial class PasswordReset
    {

        // State 
        private string loginValidationMessage = "";
        private bool loading = false;

        // Data
        private PasswordResetModel passwordResetModel = new();

        //Models
        private class PasswordResetModel
        {
            [Required(ErrorMessage = "Token richiesto")]
            public string Token { get; set; } = "";

            [Required(ErrorMessage = "Indirizzo email richiesto")]
            [EmailAddress(ErrorMessage = "Indirizzo email non valido")]
            public string Email { get; set; } = "";

            [Required(ErrorMessage = "Password richiesta")]
            [DataType(DataType.Password)]
            [StringLength(255, ErrorMessage = "La password deve essere lunga almeno 8 caratteri", MinimumLength = 8)]
            public string Password { get; set; } = "";

            [Required(ErrorMessage = "Conferma password richiesta")]
            [Compare("Password", ErrorMessage = "La password non corrisponde")]
            public string ConfirmPassword { get; set; }
        }

        protected override async Task OnInitializedAsync()
        {
            var uri = navigationManager.ToAbsoluteUri(navigationManager.Uri);
            var queryStrings = QueryHelpers.ParseQuery(uri.Query);
            if (queryStrings.TryGetValue("email", out var _email))
            {
                this.passwordResetModel.Email = _email;
            }
            else
            {
                throw new Exception();
            }

            if (queryStrings.TryGetValue("token", out var _token))
            {
                this.passwordResetModel.Token = _token;
            }
            else
            {
                throw new Exception();
            }
        }

        private async Task Submit()
        {
            loading = true;
            loginValidationMessage = "";
            StateHasChanged();

            var result = await userManager.ResetPasswordAsync(new ResetPasswordRequest
            {
                Token = passwordResetModel.Token,
                Email = passwordResetModel.Email,
                Password = passwordResetModel.Password,
            });
            loading = false;
            if (result.IsSuccess)
            {
                navigationManager.NavigateTo("/authenticate");
                return;
            }
            loginValidationMessage = result.StatusCode != System.Net.HttpStatusCode.InternalServerError ? result.Message : "Si è verificato un errore...";
            StateHasChanged();
        }
    }
}
