using Biblioconecta.Data;
using Biblioconecta.Data.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Biblioconecta.ViewModels;

public class LivrosViewModel : BaseViewModel
{
    private bool isRefreshing = false;
    private readonly BiblioconectaDatabase database;

    public ICommand DeleteCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand FavoritoCommand { get; }
    public ICommand RefreshCommand { get; }
    public ObservableCollection<Livro> Items { get; } = new();
    public bool IsRefreshing { get => isRefreshing; set => SetProperty(ref isRefreshing, value); }

    public LivrosViewModel(BiblioconectaDatabase database)
    {
        this.database = database;
        DeleteCommand = new Command<Livro>(async (value) => await DeleteItemAsync(value));
        EditCommand = new Command<Livro?>(async (value) => await EditItemAsync(value));
        FavoritoCommand = new Command<Livro>(async (value) => await FavoritoAsync(value));
        RefreshCommand = new Command(async () => await GetItemsAsync());
    }

    public async Task GetItemsAsync()
    {
        IsRefreshing = true;
        var result = await database.GetLivrosAsync();
        Items.Clear();
        foreach (var item in result.OrderBy(e => e.Titulo))
        {
            Items.Add(item);
        }
        IsRefreshing = false;
    }

    public async Task EditItemAsync(Livro? value)
    {
        await Shell.Current.GoToAsync($"//LivroEditPage?id={value?.Id ?? 0}");
    }

    public async Task FavoritoAsync(Livro value)
    {
        value.Favorito = !value.Favorito;
        await database.SaveLivroAsync(value);
        await GetItemsAsync();
    }

    public async Task DeleteItemAsync(Livro value)
    {
        var result = await Shell.Current.DisplayAlert("Remover?", "Deseja realmente remover o livro?", "Sim, eu quero", "Cancelar");
        if (result)
        {
            await database.DeleteLivroAsync(value);
            await GetItemsAsync();
        }
    }
}
