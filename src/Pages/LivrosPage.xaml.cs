using Biblioconecta.ViewModels;

namespace Biblioconecta.Pages;

public partial class LivrosPage : ContentPage
{
    public LivrosPage(LivrosViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
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
            await((LivrosViewModel)BindingContext).GetItemsAsync(string.Empty);
        }
    }
}