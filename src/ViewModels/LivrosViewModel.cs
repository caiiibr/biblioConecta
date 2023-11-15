using Biblioconecta.Data;
using Biblioconecta.Data.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Biblioconecta.ViewModels;

public class LivrosViewModel : BaseViewModel, IQueryAttributable
{
    private int prateleiraId;
    private bool isRefreshing = true;
    private string searchText = string.Empty;
    private readonly BiblioconectaDatabase database;

    public ICommand DeleteCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand FavoriteCommand { get; }
    public ICommand RefreshCommand { get; }
    public ICommand SearchCommand { get; }
    public ICommand FilterCommand { get; }

    public ObservableCollection<Livro> Items { get; } = new();
    public ObservableCollection<Prateleira> Prateleiras { get; } = new();
    public bool IsRefreshing { get => isRefreshing; set => SetProperty(ref isRefreshing, value); }
    public int PrateleiraId { get => prateleiraId; set => SetProperty(ref prateleiraId, value); }
    public string SearchText { get => searchText; set => SetProperty(ref searchText, value); }

    public LivrosViewModel(BiblioconectaDatabase database)
    {
        this.database = database;
        SearchText = string.Empty;
        DeleteCommand = new Command<Livro>(async (value) => await DeleteItemAsync(value));
        EditCommand = new Command<Livro?>(async (value) => await EditItemAsync(value));
        FavoriteCommand = new Command<Livro>(async (value) => await FavoritoAsync(value));
        RefreshCommand = new Command(async () => await GetItemsAsync(SearchText));
        SearchCommand = new Command<string>(async (string value) => await GetItemsAsync(value));
        FilterCommand = new Command<Prateleira>(async (value) => await SetPrateleira(value));
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("prateleiraId", out object? value))
        {
            if (value != null)
            {
                PrateleiraId = Convert.ToInt32(value);
                await GetItemsAsync(SearchText);
            }
        }
    }

    private async Task SetPrateleira(Prateleira value)
    {
        PrateleiraId = value.Id;
        await GetItemsAsync(SearchText);
    }

    public async Task GetPrateleiras()
    {
        Prateleiras.Clear();
        var result = await database.GetPrateleirasAsync(Settings.Usuario?.Id ?? 0);
        Prateleiras.Clear();
        foreach (var item in result)
        {
            Prateleiras.Add(item);
        }
        Prateleiras.Insert(0, new() { Id = 0, Nome = "Todos" });
    }

    public async Task GetItemsAsync(string searchText)
    {
        IsRefreshing = true;

        await GetPrateleiras();

        Items.Clear();
        var result = await database.GetLivrosAsync(Settings.Usuario?.Id ?? 0);
        if (!string.IsNullOrEmpty(searchText))
        {
            result = result.Where(e => e.Titulo.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                || e.Autor.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                || e.ISBN == searchText)
                .ToList();
        }
        if (PrateleiraId > 0)
        {
            result = result.Where(e => e.PrateleiraId == PrateleiraId).ToList();
        }
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
        await GetItemsAsync(SearchText);
    }

    public async Task DeleteItemAsync(Livro value)
    {
        var result = await Shell.Current.DisplayAlert("Remover?", "Deseja realmente remover o livro?", "Sim, eu quero", "Cancelar");
        if (result)
        {
            await database.DeleteLivroAsync(value);
            await GetItemsAsync(SearchText);
        }
    }
}
