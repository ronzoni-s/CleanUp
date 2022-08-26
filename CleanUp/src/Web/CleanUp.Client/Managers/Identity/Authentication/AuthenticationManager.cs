using Blazored.LocalStorage;
using CleanUp.Client.Authentication;
using CleanUp.Client.Constants.Storage;
using CleanUp.Client.Extensions;
using CleanUp.Client.Services.User;
using CleanUp.WebApi.Sdk.Endpoints;
using CleanUp.WebApi.Sdk.Models;
using CleanUp.WebApi.Sdk.Requests.User;
using CleanUp.Client.Models.Api;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Net;

namespace CleanUp.Client.Managers.Identity.Authentication
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly IUserService userService;

        public AuthenticationManager(
            HttpClient httpClient,
            ILocalStorageService localStorage,
            AuthenticationStateProvider authenticationStateProvider
            , IUserService userService
            )
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;
            this.userService = userService;
        }

        public async Task<ClaimsPrincipal> CurrentUser()
        {
            var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
            return state.User;
        }

        public async Task<ApiResult<LoginResponse>> Login(TokenRequest model)
        {
            var response = await _httpClient.PostAsJsonAsync(UserEndpoints.Login, model);
            var result = await response.ToResult<LoginResponse>();
            if (result.IsSuccess)
            {
                var token = result.Response.Token;
                var refreshToken = result.Response.RefreshToken;

                var jwtParsed = ClientStateProvider.GetClaimsFromJwt(token);
                var portaleRole = jwtParsed.FirstOrDefault(x => x.Value == "B2B.Portale" && x.Type == ClaimTypes.Role);
                if (portaleRole == null)
				{
                    return new ApiResult<LoginResponse>(false, HttpStatusCode.NotFound, null);
				}

                await _localStorage.SetItemAsync(StorageConstants.Local.AuthToken, token);
                await _localStorage.SetItemAsync(StorageConstants.Local.RefreshToken, refreshToken);

                ((ClientStateProvider)this._authenticationStateProvider).MarkUserAsAuthenticated(result.Response.User.FirstName);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                userService.DefaultCompanyAddressId = result.Response.User.DefaultCompanyAddressId;

                return result;
            }
            else
            {
                return result;
            }
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync(StorageConstants.Local.AuthToken);
            await _localStorage.RemoveItemAsync(StorageConstants.Local.RefreshToken);
            ((ClientStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
            userService.DefaultCompanyAddressId = null;
        }

        public async Task<string> RefreshToken()
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>(StorageConstants.Local.AuthToken);
                var refreshToken = await _localStorage.GetItemAsync<string>(StorageConstants.Local.RefreshToken);

                var response = await _httpClient.PostAsJsonAsync(UserEndpoints.RefreshToken, new RefreshTokenRequest { Token = token, RefreshToken = refreshToken });

                var result = await response.ToResult<LoginResponse>();

                if (!result.IsSuccess)
                {
                    throw new ApplicationException("Something went wrong during the refresh token action");
                }

                token = result.Response.Token;
                refreshToken = result.Response.RefreshToken;
                await _localStorage.SetItemAsync(StorageConstants.Local.AuthToken, token);
                await _localStorage.SetItemAsync(StorageConstants.Local.RefreshToken, refreshToken);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return token;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<string> TryRefreshToken()
        {
            //check if token exists
            var availableToken = await _localStorage.GetItemAsync<string>(StorageConstants.Local.RefreshToken);
            if (string.IsNullOrEmpty(availableToken)) return string.Empty;
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var exp = user.FindFirst(c => c.Type.Equals("exp"))?.Value;
            var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));
            var timeUTC = DateTime.UtcNow;
            var diff = expTime - timeUTC;
            if (diff.TotalMinutes <= 1)
                return await RefreshToken();
            return string.Empty;
        }

        public async Task<string> TryForceRefreshToken()
        {
            return await RefreshToken();
        }
    }
}