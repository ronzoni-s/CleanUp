using CleanUp.WebApi.Sdk.Models;
using CleanUp.WebApi.Sdk.Requests;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CleanUp.Client.Pages
{
    public partial class UpdateEventModal
    {
        [Parameter] public UpdateEventModel Model { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        private List<Classroom> classrooms;
        bool _loaded = false;

        public class UpdateEventModel : UpdateEventRequest
        {
            public int Id { get; set; }

            public DateTime? Date { 
                get
                {
                    return StartTime.Date;
                }
                set
                {
                    StartTime = (value?.Date ?? new DateTime()) + StartTime.TimeOfDay;
                    EndTime = (value?.Date ?? new DateTime()) + EndTime.TimeOfDay;
                }
            }

            public TimeSpan? StartHour 
            {
                get => StartTime.TimeOfDay;
                set
                {
                    StartTime = StartTime.Date + (value ?? new TimeSpan());
                }
            }
            public TimeSpan? EndHour
            {
                get => EndTime.TimeOfDay;
                set
                {
                    EndTime = EndTime.Date + (value ?? new TimeSpan());
                }
            }
        }

        private bool Validated => Model != null;

        protected override async Task OnInitializedAsync()
        {
            var response = await classroomManager.GetAllAsync();
            if (response.IsSuccess)
            {
                classrooms = response.Response.ToList();
            }
            else
            {
                snackBar.Add("Si è verificato un errore nel caricamento delle aule", Severity.Error);
            }
            _loaded = true;
        }

        private void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SubmitAsync()
        {
            var response = await eventManager.UpdateAsync(Model.Id, Model);
            if (response.IsSuccess)
            {
                snackBar.Add("Modifica effettuata con successo", Severity.Success);
                MudDialog.Close();
            }
            else
            {
                snackBar.Add("Si è verificato un errore...", Severity.Error);
            }
        }
       
    }
}