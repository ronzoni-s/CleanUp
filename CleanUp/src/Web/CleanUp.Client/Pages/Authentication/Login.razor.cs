using CleanUp.WebApi.Sdk.Requests.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CleanUp.Client.Pages.Authentication
{
    public partial class Login
    {
        private bool Validated => true; // _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private LoginRequest tokenModel = new();

        protected override async Task OnInitializedAsync()
        {
            var state = await stateProvider.GetAuthenticationStateAsync();
            if (state != new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())))
            {
                navigationManager.NavigateTo("/");
            }
        }

        private async Task SubmitAsync()
        {
            var result = await authenticationManager.Login(tokenModel);
            if (result.IsSuccess)
            {
                snackBar.Add(string.Format("Benvenuto {0}", tokenModel.Email), Severity.Success);
                navigationManager.NavigateTo("/", true);
            }
            else
            {
                
                snackBar.Add(result.Message, Severity.Error);
            }
        }

        private bool _passwordVisibility;
        private InputType _passwordInput = InputType.Password;
        private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

        void TogglePasswordVisibility()
        {
            if(_passwordVisibility)
            {
                _passwordVisibility = false;
                _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
                _passwordInput = InputType.Password;
            }
            else
            {
                _passwordVisibility = true;
                _passwordInputIcon = Icons.Material.Filled.Visibility;
                _passwordInput = InputType.Text;
            }
        }

        private void FillAdministratorCredentials()
        {
            tokenModel.Email = "mukesh@blazorhero.com";
            tokenModel.Password = "123Pa$$word!";
        }

        private void FillBasicUserCredentials()
        {
            tokenModel.Email = "john@blazorhero.com";
            tokenModel.Password = "123Pa$$word!";
        }
    }
}