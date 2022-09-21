using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CleanUp.Client.Pages
{
    public partial class UploadEventsModal
    {
        private FormModel model = new();

        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }


        private class FormModel
        {
            public MultipartFormDataContent Content { get; set; }
            public string FileName { get; set; }
        }

        private bool Validated => model?.Content != null;

        private void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            long maxFileSize = 600000;

            model.Content = new MultipartFormDataContent();

            var _file = e.File;
            try
            {
                var fileContent =
                    new StreamContent(_file.OpenReadStream(maxFileSize));

                fileContent.Headers.ContentType =
                    new MediaTypeHeaderValue(_file.ContentType);

                model.FileName = _file.Name;

                model.Content.Add(
                    content: fileContent,
                    name: "\"files\"",
                    fileName: _file.Name);
            }
            catch (Exception ex)
            {
                snackBar.Add("Si è verificato un errore...", Severity.Error);
                model.Content = null;
            }
        }

        private async Task SubmitAsync()
        {
            if (model?.Content == null)
                return;

            var response = await eventManager.UploadAsync(model.Content);
            if (response.IsSuccess)
            {
                snackBar.Add("Caricamento effettuato con successo", Severity.Success);
                MudDialog.Close();
            }
            else
            {
                snackBar.Add("Si è verificato un errore...", Severity.Error);
            }
        }
       
    }
}