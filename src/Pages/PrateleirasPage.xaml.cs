using Biblioconecta.ViewModels;

namespace Biblioconecta.Pages;

public partial class PrateleirasPage : ContentPage
{
    public PrateleirasPage(PrateleirasViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}