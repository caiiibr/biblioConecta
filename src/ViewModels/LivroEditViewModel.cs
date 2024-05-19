using Biblioconecta.Data;
using Biblioconecta.Data.Models;
using Biblioconecta.Pages;
using Biblioconecta.Services;
using Mopups.Services;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using ZXing.Net.Maui;

namespace Biblioconecta.ViewModels;

public class LivroEditViewModel : BaseViewModel, IQueryAttributable
{
    private readonly GoogleBooksService booksService;
    private readonly BiblioconectaDatabase database;
    private int id;
    private int prateleiraId;
    private string titulo = string.Empty;
    private string subtitulo = string.Empty;
    private string autor = string.Empty;
    private string editora = string.Empty;
    private string edicao = string.Empty;
    private int anoPublicacao;
    private string idioma = string.Empty;
    private int paginas;
    private string iSBN = string.Empty;
    private string descricao = string.Empty;
    private bool favorito = false;
    private bool lido = false;
    private string imagemUrl = string.Empty;
    private Prateleira? prateleira;

    public int Id { get => id; set => SetProperty(ref id, value); }
    public int PrateleiraId { get => prateleiraId; set => SetProperty(ref prateleiraId, value); }
    public string Titulo { get => titulo; set => SetProperty(ref titulo, value); }
    public string Subtitulo { get => subtitulo; set => SetProperty(ref subtitulo, value); }
    public string Autor { get => autor; set => SetProperty(ref autor, value); }
    public string Editora { get => editora; set => SetProperty(ref editora, value); }
    public string Edicao { get => edicao; set => SetProperty(ref edicao, value); }
    public int AnoPublicacao { get => anoPublicacao; set => SetProperty(ref anoPublicacao, value); }
    public string Idioma { get => idioma; set => SetProperty(ref idioma, value); }
    public int Paginas { get => paginas; set => SetProperty(ref paginas, value); }
    public string ISBN { get => iSBN; set => SetProperty(ref iSBN, value); }
    public string Descricao { get => descricao; set => SetProperty(ref descricao, value); }
    public bool Favorito { get => favorito; set => SetProperty(ref favorito, value); }
    public bool Lido { get => lido; set => SetProperty(ref lido, value); }
    public string ImagemUrl { get => imagemUrl; set => SetProperty(ref imagemUrl, value); }
    public Prateleira? Prateleira { get => prateleira; set => SetProperty(ref prateleira, value); }

    public ICommand SearchCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }
    public ObservableCollection<Prateleira> Prateleiras { get; }

    public LivroEditViewModel(
        GoogleBooksService booksService,
        BiblioconectaDatabase database)
    {
        this.booksService = booksService;
        this.database = database;
        SearchCommand = new Command(async () => await SearchAsync());
        CancelCommand = new Command(async () => await CancelAsync());
        SaveCommand = new Command(async () => await SaveAsync());
        Prateleiras = new();
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        await Reset();
        if (query.TryGetValue("id", out object? value))
        {
            if (value != null)
            {
                Id = Convert.ToInt32(value);
                var livro = await database.GetLivroAsync(Settings.Usuario?.Id ?? 0, Id);
                if (livro != null)
                {
                    PrateleiraId = livro.PrateleiraId;
                    Titulo = livro.Titulo;
                    Subtitulo = livro.Subtitulo;
                    Autor = livro.Autor;
                    Editora = livro.Editora;
                    Edicao = livro.Edicao;
                    AnoPublicacao = livro.AnoPublicacao;
                    Idioma = livro.Idioma;
                    Paginas = livro.Paginas;
                    ISBN = livro.ISBN;
                    Descricao = livro.Descricao;
                    Favorito = livro.Favorito;
                    Lido = livro.Lido;
                    ImagemUrl = livro.ImagemUrl;

                    Prateleira = Prateleiras.FirstOrDefault(e => e.Id == PrateleiraId);
                }
            }
        }
        if (query.TryGetValue("openCamera", out var openCamera))
        {
            if (openCamera != null)
            {
                if (Convert.ToBoolean(openCamera))
                {
                    await SearchAsync();
                }
            }
        }
    }

    public async Task Reset()
    {
        Prateleiras.Clear();
        var prats = await database.GetPrateleirasAsync(Settings.Usuario?.Id ?? 0);
        foreach (var item in prats)
        {
            Prateleiras.Add(item);
        }

        Id = 0;
        PrateleiraId = 0;
        Titulo = string.Empty;
        Subtitulo = string.Empty;
        Autor = string.Empty;
        Editora = string.Empty;
        Edicao = string.Empty;
        AnoPublicacao = DateTime.Today.Year;
        Idioma = string.Empty;
        Paginas = 0;
        ISBN = string.Empty;
        Descricao = string.Empty;
        Favorito = false;
        Lido = false;
        ImagemUrl = string.Empty;
    }

    public async Task SearchAsync()
    {
        if (string.IsNullOrEmpty(ISBN))
        {
            // abrir camera e escanear código de barras
            var scanner = new ScannerPopup();
            await MopupService.Instance.PushAsync(scanner);
            BarcodeResult? result = await scanner.WaitAsync();
            if (result != null)
            {
                ISBN = result.Value;
            }
        }
        if (!string.IsNullOrEmpty(ISBN))
        {
            var book = await booksService.GetAsync(ISBN);
            if (book != null)
            {
                Titulo = book.VolumeInfo.Title ?? "";
                Subtitulo = book.VolumeInfo.Subtitle ?? "";
                Autor = string.Join(", ", book.VolumeInfo.Authors);
                Editora = book.VolumeInfo.Publisher ?? "";
                Edicao = book.VolumeInfo.ContentVersion ?? "";
                AnoPublicacao = (book.VolumeInfo.PublishedDate ?? DateTime.Today).Year;
                Paginas = book.VolumeInfo.PageCount ?? 0;
                Descricao = book.VolumeInfo.Description ?? "";
                ImagemUrl = book.VolumeInfo.ImageLinks.Thumbnail
                    ?? book.VolumeInfo.ImageLinks.SmallThumbnail
                    ?? "";

                if (!string.IsNullOrEmpty(book.VolumeInfo.Language))
                {
                    CultureInfo cultureInfo = new(book.VolumeInfo.Language);
                    Idioma = cultureInfo.NativeName;
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Algo deu errado...", "Não encontramos nenhum livro com esse código de ISBN.", "Ok, entendi");
            }
        }
    }

    public async Task CancelAsync()
    {
        await Reset();
        await Shell.Current.GoToAsync("//LivrosPage");
    }

    public async Task SaveAsync()
    {
        if ((Prateleira?.Id ?? 0) < 1)
        {
            await Shell.Current.DisplayAlert("Algo deu errado...", "A prateleira é obrigatória.", "Ok, entendi");
            return;
        }

        if (string.IsNullOrEmpty(ISBN))
        {
            await Shell.Current.DisplayAlert("Algo deu errado...", "O ISBN é obrigatório.", "Ok, entendi");
            return;
        }

        if (string.IsNullOrEmpty(Titulo))
        {
            await Shell.Current.DisplayAlert("Algo deu errado...", "O título é obrigatório.", "Ok, entendi");
            return;
        }

        Livro value = new()
        {
            Id = Id,
            UsuarioId = Settings.Usuario?.Id ?? 0,
            PrateleiraId = Prateleira?.Id ?? 0,
            Titulo = Titulo,
            Subtitulo = Subtitulo,
            Autor = Autor,
            Editora = Editora,
            Edicao = Edicao,
            AnoPublicacao = AnoPublicacao,
            Idioma = Idioma,
            Paginas = Paginas,
            ISBN = ISBN,
            Descricao = Descricao,
            Favorito = Favorito,
            Lido = Lido,
            ImagemUrl = ImagemUrl
        };
        await database.CreateOrUpdateLivroAsync(value);
        await Reset();
        await Shell.Current.GoToAsync("//LivrosPage");
    }
}
