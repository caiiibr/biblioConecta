using Biblioconecta.ViewModels;

namespace Biblioconecta.Pages;

public partial class MetasPage : ContentPage
{
    public MetasPage(MetasViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((MetasViewModel)BindingContext).GetItemsAsync();
    }
}