using ErbertPranzi.Shared.Settings;
using System.Threading.Tasks;
using ErbertPranzi.Shared.Wrapper;

namespace ErbertPranzi.Shared.Managers
{
    public interface IPreferenceManager
    {
        Task SetPreference(IPreference preference);

        Task<IPreference> GetPreference();

        Task<IResult> ChangeLanguageAsync(string languageCode);
    }
}