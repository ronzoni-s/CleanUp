using System.Collections.Generic;
using Blazored.LocalStorage;
using CleanUp.Client.Settings;
using System.Threading.Tasks;
using CleanUp.Client.Models.Api;
using CleanUp.Client.Constants.Storage;
using System.Net;

namespace CleanUp.Client.Managers.Preferences
{
    public class ClientPreferenceManager : IClientPreferenceManager
    {
        private readonly ILocalStorageService _localStorageService;

        public ClientPreferenceManager(
            ILocalStorageService localStorageService
            )
        {
            _localStorageService = localStorageService;
        }       

        //public async Task<ApiResult> ChangeLanguageAsync(string languageCode)
        //{
        //    var preference = await GetPreference() as ClientPreference;
        //    if (preference != null)
        //    {
        //        //preference.LanguageCode = languageCode;
        //        await SetPreference(preference);
        //        return new ApiResult(true, HttpStatusCode.OK, "Client Language has been changed");
        //    }

        //    return new ApiResult(false, HttpStatusCode.InternalServerError, _localizer["Failed to get client preferences"]);
        //}

        public async Task<IPreference> GetPreference()
        {
            return await _localStorageService.GetItemAsync<ClientPreference>(StorageConstants.Local.Preference) ?? new ClientPreference();
        }

        public async Task SetPreference(IPreference preference)
        {
            await _localStorageService.SetItemAsync(StorageConstants.Local.Preference, preference as ClientPreference);
        }
    }
}