using Biblioconecta.ViewModels;

namespace Biblioconecta.Pages;

public partial class LivroEditPage : ContentPage
{
    public LivroEditPage(LivroEditViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((LivroEditViewModel)BindingContext).Init();
    }
}