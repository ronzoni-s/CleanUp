using System.Linq;
using ErbertPranzi.Shared.Constants.Localization;
using ErbertPranzi.Shared.Settings;

namespace ErbertPranzi.Client.Infrastructure.Settings
{
    public record ClientPreference : IPreference
    {
        public bool IsDarkMode { get; set; }
        public bool IsRTL { get; set; }
        public bool IsDrawerOpen { get; set; }
        public string PrimaryColor { get; set; }
        public string LanguageCode { get; set; } = LocalizationConstants.SupportedLanguages.FirstOrDefault()?.Code ?? "it-IT";
    }
}