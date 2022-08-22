using System.Linq;
using ErbertPranzi.Shared.Constants.Localization;
using ErbertPranzi.Shared.Settings;

namespace ErbertPranzi.Server.Settings
{
    public record ServerPreference : IPreference
    {
        public string LanguageCode { get; set; } = LocalizationConstants.SupportedLanguages.FirstOrDefault()?.Code ?? "it-IT";

        //TODO - add server preferences
    }
}