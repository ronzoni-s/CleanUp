using System.Linq;
using CleanUp.Shared.Constants.Localization;
using CleanUp.Shared.Settings;

namespace CleanUp.Server.Settings
{
    public record ServerPreference : IPreference
    {
        public string LanguageCode { get; set; } = LocalizationConstants.SupportedLanguages.FirstOrDefault()?.Code ?? "it-IT";

        //TODO - add server preferences
    }
}