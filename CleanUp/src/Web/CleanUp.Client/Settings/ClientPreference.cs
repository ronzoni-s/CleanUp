using CleanUp.Client.Constants.Localization;
using System.Linq;
//using CleanUp.Client.Constants.Localization;

namespace CleanUp.Client.Settings
{
    public record ClientPreference : IPreference
    {
        public string LanguageCode { get; set; } = LocalizationConstants.SupportedLanguages.FirstOrDefault()?.Code ?? "it-IT";
        public bool? AcceptCookies { get; set; }
    }
}