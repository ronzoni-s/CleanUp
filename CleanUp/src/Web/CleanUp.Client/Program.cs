using CleanUp.Client;
using CleanUp.Client.Constants.Localization;
using CleanUp.Client.Extensions;
using CleanUp.Client.Managers.Preferences;
using CleanUp.Client.Settings;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Globalization;

var builder = WebAssemblyHostBuilder
                .CreateDefault(args)
                .AddRootComponents()
                .AddClientServices();


//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
var host = builder.Build();
//var storageService = host.Services.GetRequiredService<ClientPreferenceManager>();
//if (storageService != null)
//{
//    CultureInfo culture;
//    var preference = await storageService.GetPreference() as ClientPreference;
//    if (preference != null)
//        culture = new CultureInfo(preference.LanguageCode);
//    else
//        culture = new CultureInfo(LocalizationConstants.SupportedLanguages.FirstOrDefault()?.Code ?? "it-IT");
//    CultureInfo.DefaultThreadCurrentCulture = culture;
//    CultureInfo.DefaultThreadCurrentUICulture = culture;
//}
await builder.Build().RunAsync();
