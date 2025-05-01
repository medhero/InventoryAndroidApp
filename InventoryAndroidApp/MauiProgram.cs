using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Maui;
using InventoryAndroidApp.Services;
using InventoryAndroidApp.ViewModels;
using InventoryAndroidApp.Views;
using System.Net.Http.Headers;

namespace InventoryAndroidApp;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        }).UseMauiCommunityToolkit();

        // Register HttpClient with proper configuration
        builder.Services.AddHttpClient<IInventoryApiService, InventoryApiService>(client =>
        {
            client.BaseAddress = new Uri("http://10.0.2.2:5219");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        });



        // ✅ Dependency Injection - ViewModels
        builder.Services.AddTransient<ItemDetailViewModel>();
        builder.Services.AddTransient<ItemsViewModel>();

        // ✅ Dependency Injection - Pages
        builder.Services.AddTransient<ItemDetailPage>();
        builder.Services.AddTransient<ItemsPage>();
        builder.Services.AddTransient<AddItemPage>();
        builder.Services.AddSingleton<AppShell>();

#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}