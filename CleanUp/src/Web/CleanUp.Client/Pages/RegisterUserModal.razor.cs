using CleanUp.WebApi.Sdk.Models;
using CleanUp.WebApi.Sdk.Requests;
using CleanUp.WebApi.Sdk.Requests.User;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CleanUp.Client.Pages
{
    public partial class RegisterUserModal
    {
        [Parameter] public RegisterUserRequest Model { get; set; } = new()
        {
            EmailConfirmed = true
        };
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private bool Validated => Model != null;


        private void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SubmitAsync()
        {
            var response = await userManager.RegisterAsync(Model);
            if (response.IsSuccess)
            {
                snackBar.Add("Registrazione effettuata con successo", Severity.Success);
                MudDialog.Close();
            }
            else
            {
                snackBar.Add("Si è verificato un errore...", Severity.Error);
            }
        }
       
    }
}