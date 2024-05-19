using Biblioconecta.Data;
using Biblioconecta.Pages;
using Biblioconecta.Services;
using Biblioconecta.ViewModels;
using Microsoft.Extensions.Logging;
using Mopups.Hosting;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;
#if ANDROID
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
#endif

namespace Biblioconecta
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureMopups()
                .UseBarcodeReader()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemiBold");
                    fonts.AddFont("Font-Awesome-6-Free-Solid-900.otf", "FontAwesomeSolid");
                    fonts.AddFont("Font-Awesome-6-Free-Regular-400.otf", "FontAwesomeRegular");
                    fonts.AddFont("Abel-Regular.ttf", "AbelRegular");
                    fonts.AddFont("Poppins-Bold.ttf", "PoppinsBold");
                    fonts.AddFont("Poppins-Light.ttf", "PoppinsLight");
                    fonts.AddFont("Poppins-Regular.ttf", "PoppinsRegular");
                    fonts.AddFont("Poppins-SemiBold.ttf", "PoppinsSemiBold");
                    fonts.AddFont("Kameron-Bold.ttf", "KameronBold");
                })
                .ConfigureMauiHandlers(h =>
                {
                    h.AddHandler(typeof(ZXing.Net.Maui.Controls.CameraBarcodeReaderView), typeof(CameraBarcodeReaderViewHandler));
                    h.AddHandler(typeof(ZXing.Net.Maui.Controls.CameraView), typeof(CameraViewHandler));
                    h.AddHandler(typeof(ZXing.Net.Maui.Controls.BarcodeGeneratorView), typeof(BarcodeGeneratorViewHandler));
                });

            builder.Services.AddScoped<LivrosPopup>();

            builder.Services.AddTransient<FavoritosPage>();
            builder.Services.AddTransient<InicioPage>();
            builder.Services.AddTransient<LivroEditPage>();
            builder.Services.AddTransient<LivrosPage>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<MetasPage>();
            builder.Services.AddTransient<MetaEditPage>();
            builder.Services.AddTransient<MinhaContaPage>();
            builder.Services.AddTransient<PrateleirasPage>();
            builder.Services.AddTransient<RedirectToNewPage>();
            builder.Services.AddTransient<RedirectToScannerPage>();
            builder.Services.AddTransient<RegistroPage>();

            builder.Services.AddSingleton<FavoritosViewModel>();
            builder.Services.AddSingleton<LivroEditViewModel>();
            builder.Services.AddSingleton<LivrosPopupViewModel>();
            builder.Services.AddSingleton<LivrosViewModel>();
            builder.Services.AddSingleton<MetasViewModel>();
            builder.Services.AddSingleton<MetaEditViewModel>();
            builder.Services.AddSingleton<PrateleirasViewModel>();

            builder.Services.AddSingleton<BiblioconectaDatabase>();

            builder.Services.AddHttpClient<GoogleBooksService>();

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
        Microsoft.Maui.Handlers.SearchBarHandler.Mapper.AppendToMapping("NoUnderline", (h, v) =>
        {
            // Remove underline:
            h.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToAndroid());
        });
#endif

            return builder.Build();
        }
    }
}
