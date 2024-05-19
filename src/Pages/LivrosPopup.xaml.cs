using Biblioconecta.Data.Models;
using Biblioconecta.ViewModels;
using Mopups.Pages;
using Mopups.Services;
using System.Runtime.CompilerServices;

namespace Biblioconecta.Pages;

public partial class LivrosPopup : PopupPage
{
    private TaskCompletionSource<Livro?> taskCompletionSource;
    public Task<Livro?> WaitAsync() => taskCompletionSource.Task;

    public LivrosPopup(LivrosPopupViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        taskCompletionSource = new();
    }

    protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null!)
    {
        if (EqualityComparer<T>.Default.Equals(storage, value))
        {
            return false;
        }
        storage = value;
        OnPropertyChanged(propertyName);

        return true;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((LivrosPopupViewModel)BindingContext).GetItemsAsync(string.Empty);
    }

    private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(e.NewTextValue))
        {
            await ((LivrosPopupViewModel)BindingContext).GetItemsAsync(string.Empty);
        }
    }

    private async void Cancelar_Clicked(object sender, EventArgs e)
    {
        taskCompletionSource.SetResult(null);
        await MopupService.Instance.PopAsync(true);
    }

    private async void Livro_Tapped(object sender, TappedEventArgs e)
    {
        taskCompletionSource.SetResult(e.Parameter as Livro);
        await MopupService.Instance.PopAsync(true);
    }
}