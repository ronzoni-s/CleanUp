using CleanUp.Shared.Settings;
using System.Threading.Tasks;
using CleanUp.Shared.Wrapper;

namespace CleanUp.Shared.Managers
{
    public interface IPreferenceManager
    {
        Task SetPreference(IPreference preference);

        Task<IPreference> GetPreference();

        Task<IResult> ChangeLanguageAsync(string languageCode);
    }
}