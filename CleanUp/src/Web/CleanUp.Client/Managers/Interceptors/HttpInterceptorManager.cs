using CleanUp.Client.Managers.Identity.Authentication;
using Microsoft.AspNetCore.Components;
using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Toolbelt.Blazor;
using CleanUp.Client.Managers.Catalogs;
using CleanUp.Client.Managers.Orders;

namespace CleanUp.Client.Managers.Interceptors
{
    public class HttpInterceptorManager : IHttpInterceptorManager
    {
        private readonly HttpClientInterceptor _interceptor;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly ICatalogManager catalogManager;
        private readonly NavigationManager _navigationManager;
        //private readonly ISnackbar _snackBar;

        public HttpInterceptorManager(
            HttpClientInterceptor interceptor
            , IAuthenticationManager authenticationManager
            , ICatalogManager catalogManager
            , NavigationManager navigationManager
            )
        {
            _interceptor = interceptor;
            _authenticationManager = authenticationManager;
            this.catalogManager = catalogManager;
            _navigationManager = navigationManager;
        }

        public void RegisterEvent() => _interceptor.BeforeSendAsync += InterceptBeforeHttpAsync;

        public async Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs e)
        {
            var absPath = e.Request.RequestUri.AbsolutePath;
            if (!absPath.Contains("token") && !absPath.Contains("user"))
            {
                try
                {
                    var token = await _authenticationManager.TryRefreshToken();
                    if (!string.IsNullOrEmpty(token))
                    {
                        //_snackBar.Add(_localizer["Refreshed Token."], Severity.Success);
                        Console.WriteLine("Token refreshed");
                        e.Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Logged out - {ex.Message}");
                    //_snackBar.Add(_localizer["You are Logged Out."], Severity.Error);
                    await _authenticationManager.Logout();
                    await catalogManager.GetCatalog(true);
                    _navigationManager.NavigateTo("/authenticate");
                }
            }
        }

        public void DisposeEvent() => _interceptor.BeforeSendAsync -= InterceptBeforeHttpAsync;
    }
}