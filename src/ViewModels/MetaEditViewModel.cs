using Biblioconecta.Data;
using Biblioconecta.Data.Models;
using Biblioconecta.Pages;
using Mopups.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Biblioconecta.ViewModels
{
    public class MetaEditViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly IServiceProvider serviceProvider;
        private readonly BiblioconectaDatabase database;

        private int id;
        private string nome = "";
        private DateTime dataInicio = DateTime.Today;
        private DateTime dataTermino = DateTime.Today.AddMonths(1);
        private int quantidadeLivros = 1;
        private int quantidadeLivrosLidos;

        public int Id { get => id; set => SetProperty(ref id, value); }
        public string Nome { get => nome; set => SetProperty(ref nome, value); }
        public DateTime DataInicio { get => dataInicio; set => SetProperty(ref dataInicio, value); }
        public DateTime DataTermino { get => dataTermino; set => SetProperty(ref dataTermino, value); }
        public int QuantidadeLivros { get => quantidadeLivros; set => SetProperty(ref quantidadeLivros, value); }
        public int QuantidadeLivrosLidos { get => quantidadeLivrosLidos; set => SetProperty(ref quantidadeLivrosLidos, value); }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand SelectItemCommand { get; }

        public ObservableCollection<Livro> Livros { get; }


        public MetaEditViewModel(
            IServiceProvider serviceProvider,
            BiblioconectaDatabase database)
        {
            this.serviceProvider = serviceProvider;
            this.database = database;
            Livros = new ObservableCollection<Livro>();
            CancelCommand = new Command(async () => await CancelAsync());
            SaveCommand = new Command(async () => await SaveAsync());
            SelectItemCommand = new Command<Livro>(async (value) => await SelectItemAsync(value));
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            await Reset();
            if (query.TryGetValue("id", out object? value))
            {
                if (value != null)
                {
                    Id = Convert.ToInt32(value);
                    var meta = await database.GetMetaAsync(Settings.Usuario?.Id ?? 0, Id);
                    if (meta != null)
                    {
                        Nome = meta.Nome;
                        DataInicio = meta.DataInicio;
                        DataTermino = meta.DataTermino;
                        QuantidadeLivros = meta.QuantidadeLivros;
                        QuantidadeLivrosLidos = meta.QuantidadeLivrosLidos;
                    }

                    var livros = await database.GetLivrosByMetaAsync(Settings.Usuario?.Id ?? 0, Id);
                    Livros.Clear();
                    foreach (var item in livros)
                    {
                        Livros.Add(item);
                    }
                    Livros.Add(new Livro { Id = 0 });
                }
            }
        }

        public async Task Reset()
        {
            Livros.Clear();

            Id = 0;
            Nome = "";
            DataInicio = DateTime.Today;
            DataTermino = DateTime.Today.AddMonths(1);
            QuantidadeLivros = 1;
            QuantidadeLivrosLidos = 0;

            await Task.CompletedTask;
        }

        public async Task SelectItemAsync(Livro value)
        {
            if (value.Id == 0)
            {
                await AddItemAsync();
            }
            else
            {
                await DeleteItemAsync(value);
            }
        }

        public async Task AddItemAsync()
        {
            //var popup = new LivrosPopup();
            using var scope = serviceProvider.CreateScope();
            var popup = scope.ServiceProvider.GetRequiredService<LivrosPopup>();
            await MopupService.Instance.PushAsync(popup);
            Livro? result = await popup.WaitAsync();
            if (result != null)
            {
                Livros.Insert(Livros.Count - 1, result);
            }
        }

        public async Task DeleteItemAsync(Livro value)
        {
            if (value.Id > 0)
            {
                var result = await Shell.Current.DisplayAlert("Remover?", "Deseja realmente remover o livro?", "Sim, eu quero", "Cancelar");
                if (result)
                {
                    Livros.Remove(value);
                }
            }
        }

        public async Task CancelAsync()
        {
            await Reset();
            await Shell.Current.GoToAsync("//MetasPage");
        }

        public async Task SaveAsync()
        {
            if (string.IsNullOrEmpty(Nome))
            {
                await Shell.Current.DisplayAlert("Algo deu errado...", "O nome é obrigatório.", "Ok, entendi");
                return;
            }

            if (QuantidadeLivros < 1)
            {
                await Shell.Current.DisplayAlert("Algo deu errado...", "A quantidade de livros deve ser maior que zero.", "Ok, entendi");
                return;
            }

            Meta value = new()
            {
                Id = Id,
                UsuarioId = Settings.Usuario?.Id ?? 0,
                Nome = Nome,
                DataInicio = DataInicio,
                DataTermino = DataTermino,
                QuantidadeLivros = QuantidadeLivros,
                QuantidadeLivrosLidos = Livros.Where(e => e.Id > 0).Count()
            };
            await database.CreateOrUpdateMetaAsync(value);
            if (value.Id > 0)
            {
                await database.DeleteMetaLivroByMetaAsync(value.Id);
                foreach (var item in Livros.Where(e => e.Id > 0))
                {
                    await database.CreateMetaLivroAsync(new MetaLivro { LivroId = item.Id, MetaId = value.Id });
                }
            }
            await Reset();
            await Shell.Current.GoToAsync("//MetasPage");
        }
    }
}
