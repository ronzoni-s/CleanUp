//using CleanUp.Client.Managers.Identity.Users;
//using CleanUp.WebApi.Sdk.Models;
//using CleanUp.WebApi.Sdk.Requests;
//using CleanUp.WebApi.Sdk.Requests.User;
//using Microsoft.AspNetCore.Components;
//using Microsoft.AspNetCore.Components.Forms;
//using MudBlazor;
//using System.Net.Http.Headers;
//using System.Threading.Tasks;

//namespace CleanUp.Client.Pages
//{
//    public partial class AddWorkDayModal
//    {
//        [Parameter] public AddWorkDayRequest Model { get; set; } = new();
//        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

//        private void Cancel()
//        {
//            MudDialog.Cancel();
//        }

//        private async Task submitasync()
//        {
//            var response = await userManager.CreateWorkDay(Model.Id, Model);
//            if (response.issuccess)
//            {
//                snackbar.add("modifica effettuata con successo", severity.success);
//                MudDialog.close();
//            }
//            else
//            {
//                snackbar.add("si è verificato un errore...", severity.error);
//            }
//        }

//    }
//}