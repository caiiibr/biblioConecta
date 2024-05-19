using Biblioconecta.ViewModels;

namespace Biblioconecta.Pages;

public partial class LivrosPage : ContentPage
{
    private HttpClient httpClient;

    public LivrosPage(LivrosViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        httpClient = new HttpClient();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((LivrosViewModel)BindingContext).GetItemsAsync(string.Empty);
    }

    private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(e.NewTextValue))
        {
            await ((LivrosViewModel)BindingContext).GetItemsAsync(string.Empty);
        }
    }
}