using Biblioconecta.Data;
using Biblioconecta.Data.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Biblioconecta.ViewModels;

public class PrateleirasViewModel : BaseViewModel
{
    private bool isRefreshing = true;
    private readonly BiblioconectaDatabase database;

    public ICommand DeleteCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand LivrosCommand { get; }
    public ICommand RefreshCommand { get; }
    public ObservableCollection<Prateleira> Items { get; } = new();
    public bool IsRefreshing { get => isRefreshing; set => SetProperty(ref isRefreshing, value); }

    public PrateleirasViewModel(BiblioconectaDatabase database)
    {
        this.database = database;
        DeleteCommand = new Command<Prateleira>(async (value) => await DeleteItemAsync(value));
        EditCommand = new Command<Prateleira?>(async (value) => await EditItemAsync(value));
        LivrosCommand = new Command<Prateleira>(async (value) => await GoToLivros(value));
        RefreshCommand = new Command(async () => await GetItemsAsync());
    }

    public async Task GetItemsAsync()
    {
        IsRefreshing = true;
        var result = await database.GetPrateleirasAsync(Settings.Usuario?.Id ?? 0);
        Items.Clear();
        foreach (var item in result.OrderBy(e => e.Nome))
        {
            Items.Add(item);
        }
        IsRefreshing = false;
    }

    public async Task EditItemAsync(Prateleira? value)
    {
        string titulo = value == null ? "Nova prateleira" : "Alterar prateleira";
        var result = await Shell.Current.DisplayPromptAsync(titulo, "Nome da prateleira:", accept: "Salvar", cancel: "Cancelar", placeholder: "Informe o nome da prateleira", initialValue: value?.Nome);
        if (!string.IsNullOrWhiteSpace(result))
        {
            value = value ?? new();
            value.UsuarioId = Settings.Usuario?.Id ?? 0;
            value.Nome = result;

            await database.SavePrateleiraAsync(value);
            await GetItemsAsync();
        }
    }

    public async Task DeleteItemAsync(Prateleira value)
    {
        var result = await Shell.Current.DisplayAlert("Remover?", "Deseja realmente remover a prateleira?", "Sim, eu quero", "Cancelar");
        if (result)
        {
            await database.DeletePrateleiraAsync(value);
            await GetItemsAsync();
        }
    }

    public async Task GoToLivros(Prateleira value)
    {
        await Shell.Current.GoToAsync($"//LivrosPage?prateleiraId={value.Id}");
    }
}
