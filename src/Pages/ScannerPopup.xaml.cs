using Mopups.Pages;
using Mopups.Services;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;

namespace Biblioconecta.Pages;

public partial class ScannerPopup : PopupPage
{
    private TaskCompletionSource<BarcodeResult?> taskCompletionSource;
    public Task<BarcodeResult?> WaitAsync() => taskCompletionSource.Task;

    public ScannerPopup()
    {
        InitializeComponent();
        taskCompletionSource = new();
        barcodeReader.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormats.All,
            AutoRotate = true,
            Multiple = false
        };
    }

    private async void Close_Clicked(object sender, EventArgs e)
    {
        taskCompletionSource.SetResult(null);
        await MopupService.Instance.PopAsync(true);
    }

    private void barcodeReader_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        Dispatcher.Dispatch(async () =>
        {

            if (!taskCompletionSource.Task.IsCompleted)
            {
                if (taskCompletionSource.TrySetResult(e.Results.FirstOrDefault()))
                    await MopupService.Instance.PopAsync(true);
            }
        });
    }

    private async void Manual_Clicked(object sender, EventArgs e)
    {
        string prompt = await DisplayPromptAsync("Leitura manual", "Informe o código de barras:", accept: "Ok", cancel: "Cancelar");
        if (!string.IsNullOrEmpty(prompt))
        {
            BarcodeResult result = new() { Format = BarcodeFormat.Ean13, Value = prompt };
            if (!taskCompletionSource.Task.IsCompleted)
            {
                if (taskCompletionSource.TrySetResult(result))
                    await MopupService.Instance.PopAsync(true);
            }
        }
    }
}