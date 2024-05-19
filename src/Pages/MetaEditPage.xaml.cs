using Biblioconecta.ViewModels;

namespace Biblioconecta.Pages;

public partial class MetaEditPage : ContentPage
{
    public MetaEditPage(MetaEditViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}