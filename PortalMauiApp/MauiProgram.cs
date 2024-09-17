using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using PortalMauiApp.Data;
using PortalMauiApp.Services;

namespace PortalMauiApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

         

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif
            // set up authorization
            builder.Services.AddAuthorizationCore();

            // register the custom state provider
            builder.Services.AddScoped<AuthenticationStateProvider, AccountService>();

            // register the account management interface
            builder.Services.AddScoped(
                sp => (IAccountService)sp.GetRequiredService<AuthenticationStateProvider>());

            builder.Services.AddMauiBlazorWebView();
            //var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"PortalMauiAppDB.db3");
            builder.Services.AddSingleton<DataContext>();
            builder.Services.AddSingleton<IAccountRepository, AccountRepository>();
            builder.Services.AddSingleton<IAccountService, AccountService>();


            return builder.Build();
        }
    }
}
