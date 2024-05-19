using Biblioconecta.Data;
using Biblioconecta.Data.Models;
using System.Collections.ObjectModel;
using System.Text;
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
    public ICommand ShareCommand { get; }
    public ObservableCollection<Prateleira> Items { get; } = new();
    public bool IsRefreshing { get => isRefreshing; set => SetProperty(ref isRefreshing, value); }

    public PrateleirasViewModel(BiblioconectaDatabase database)
    {
        this.database = database;
        DeleteCommand = new Command<Prateleira>(async (value) => await DeleteItemAsync(value));
        EditCommand = new Command<Prateleira?>(async (value) => await EditItemAsync(value));
        LivrosCommand = new Command<Prateleira>(async (value) => await GoToLivros(value));
        RefreshCommand = new Command(async () => await GetItemsAsync());
        ShareCommand = new Command<Prateleira>(async (value) => await ShareAsync(value));
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

            await database.CreateOrUpdatePrateleiraAsync(value);
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

    public async Task ShareAsync(Prateleira value)
    {
        var livros = await database.GetLivrosByPrateleiraAsync(Settings.Usuario?.Id ?? 0, value.Id);

        if (livros.Count > 0)
        {
            var builder = new StringBuilder();

            builder.AppendLine($"Sejam bem-vindos à prateleira {value.Nome} do Biblioconecta!");
            builder.AppendLine("");
            builder.AppendLine("Como parte do nosso compromisso em enriquecer seu conhecimento e promover uma cultura de leitura, gostaríamos de compartilhar a lista de livros selecionados. Estas obras foram cuidadosamente escolhidas para estimular seu pensamento crítico, expandir seus horizontes e proporcionar uma experiência enriquecedora de aprendizado.");
            builder.AppendLine("");
            builder.AppendLine("Aqui está a lista de lisvros indicados:");
            foreach (var item in livros)
            {
                builder.AppendLine("");
                builder.AppendLine($"{item.Titulo}");

                if (!string.IsNullOrEmpty(item.Subtitulo))
                    builder.AppendLine($"{item.Subtitulo}");

                if (!string.IsNullOrEmpty(item.Autor))
                    builder.AppendLine($"Autores: {item.Autor}");

                if (!string.IsNullOrEmpty(item.ISBN))
                    builder.AppendLine($"ISBN: {item.ISBN}");
            }
            builder.AppendLine("");
            builder.AppendLine("");
            builder.AppendLine("Atenciosamente,");
            builder.AppendLine(Settings.Usuario?.Nome);

            string text = builder.ToString();
            await Share.Default.RequestAsync(new ShareTextRequest
            {
                Text = text,
                Title = value.Nome
            });
        }
        else
        {
            await Shell.Current.DisplayAlert("Compartilhamento...", "A prateleira selecionada não possui nenhum livro.", "Ok, entendi");
        }
    }
}
