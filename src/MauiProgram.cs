using Biblioconecta.Data;
using Biblioconecta.Pages;
using Biblioconecta.ViewModels;
using Microsoft.Extensions.Logging;
#if ANDROID
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
#endif

namespace Biblioconecta;

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
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Font-Awesome-6-Free-Solid-900.otf", "FontAwesomeSolid");
                fonts.AddFont("Font-Awesome-6-Free-Regular-400.otf", "FontAwesomeRegular");
            });
#if DEBUG
        builder.Logging.AddDebug();
#endif
#if ANDROID
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoUnderline", (h, v) =>
            {
                // Remove underline:
                h.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToAndroid());
            });
            Microsoft.Maui.Handlers.EditorHandler.Mapper.AppendToMapping("NoUnderline", (h, v) =>
            {
                // Remove underline:
                h.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToAndroid());
            });
#endif

        builder.Services.AddSingleton<FavoritosPage>();
        builder.Services.AddSingleton<LivrosPage>();
        builder.Services.AddSingleton<PrateleirasPage>();

        builder.Services.AddScoped<LivroEditPage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegistroPage>();

        builder.Services.AddSingleton<PrateleirasViewModel>();
        builder.Services.AddSingleton<FavoritosViewModel>();
        builder.Services.AddSingleton<LivrosViewModel>();
        builder.Services.AddScoped<LivroEditViewModel>();

        builder.Services.AddSingleton<BiblioconectaDatabase>();

        return builder.Build();
    }
}
