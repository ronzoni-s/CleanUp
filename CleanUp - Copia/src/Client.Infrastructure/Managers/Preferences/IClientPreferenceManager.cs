using ErbertPranzi.Shared.Managers;
using MudBlazor;
using System.Threading.Tasks;

namespace ErbertPranzi.Client.Infrastructure.Managers.Preferences
{
    public interface IClientPreferenceManager : IPreferenceManager
    {
        Task<MudTheme> GetCurrentThemeAsync();

        Task<bool> ToggleDarkModeAsync();
    }
}