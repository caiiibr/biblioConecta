using Biblioconecta.ViewModels;

namespace Biblioconecta.Pages;

public partial class FavoritosPage : ContentPage
{
    public FavoritosPage(FavoritosViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }



    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((FavoritosViewModel)BindingContext).GetItemsAsync();
    }
}