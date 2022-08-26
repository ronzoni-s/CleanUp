using CleanUp.WebApi.Sdk.Requests.User;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

namespace CleanUp.Client.Pages
{
    public partial class Authenticate
    {

        // State 
        private string loginValidationMessage = "";
        private bool loading = false;

        // Data
        private LoginModel loginModel = new();

        //Models
        private class LoginModel
        {
            [Required(ErrorMessage = "Indirizzo email richiesto")]
            [EmailAddress(ErrorMessage ="Indirizzo email non valido")]
            public string Email { get; set; } = "";
            [Required(ErrorMessage = "Password richiesta")]
            public string Password { get; set; } = "";
        }

        protected override async Task OnInitializedAsync()
        {
           
        }

        private async Task SubmitLogin()
        {
            loading = true;
            loginValidationMessage = "";
            StateHasChanged();

            var result = await authenticationManager.Login(new TokenRequest
            {
                Email = loginModel.Email,
                Password = loginModel.Password,
            });
            if (result.IsSuccess)
            {
                navigationManager.NavigateTo("/");
                return;
            }
            loading = false;
            loginValidationMessage = result.StatusCode == System.Net.HttpStatusCode.NotFound || result.StatusCode == System.Net.HttpStatusCode.Unauthorized ? "Password o email errata. Se non ricordi la password, clicca qui sopra per recuperarla" : "Si è verificato un errore...";
            StateHasChanged();
        }



    }
}
