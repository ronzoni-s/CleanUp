using CleanUp.Client.Settings;
using CleanUp.Client.Models.Api;

namespace CleanUp.Client.Managers
{
    public interface IPreferenceManager
    {
        Task SetPreference(IPreference preference);

        Task<IPreference> GetPreference();

        //Task<ApiResult> ChangeLanguageAsync(string languageCode);
    }
}