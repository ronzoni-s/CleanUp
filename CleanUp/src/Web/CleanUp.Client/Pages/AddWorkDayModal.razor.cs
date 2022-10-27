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
    public partial class AddWorkDayModal
    {
        [Parameter] public AddWorkDayModel Model { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private bool Validated => Model != null;

        public class AddWorkDayModel : AddWorkDayRequest
        {
            public DateTime? Date
            {
                get => this.Start.Date;
                set
                {
                    this.Start = value.Value.Date + this.Start.TimeOfDay;
                    this.End = value.Value.Date + this.End.TimeOfDay;
                }
            }

            public TimeSpan? StartTime
            {
                get => this.Start.TimeOfDay;
                set
                {
                    this.Start = this.Start.Date + value.Value;
                }
            }

            public TimeSpan? EndTime
            {
                get => this.End.TimeOfDay;
                set
                {
                    this.End = this.End.Date + value.Value;
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
        }

        private void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SubmitAsync()
        {
            var response = await userManager.CreateWorkDay(Model);
            if (response.IsSuccess)
            {
                snackBar.Add("Inserimento effettuato con successo", Severity.Success);
                MudDialog.Close();
            }
            else
            {
                snackBar.Add("Si è verificato un errore...", Severity.Error);
            }
        }
       
    }
}