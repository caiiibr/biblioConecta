using Biblioconecta.Data;
using Biblioconecta.Data.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Biblioconecta.ViewModels
{
    public class LivrosPopupViewModel : BaseViewModel
    {
        private bool isRefreshing = true;
        private string searchText = string.Empty;
        private BiblioconectaDatabase database;

        public ICommand RefreshCommand { get; }
        public ICommand SearchCommand { get; }

        public ObservableCollection<Livro> Items { get; }
        public bool IsRefreshing { get => isRefreshing; set => SetProperty(ref isRefreshing, value); }
        public string SearchText { get => searchText; set => SetProperty(ref searchText, value); }

        public LivrosPopupViewModel(BiblioconectaDatabase database)
        {
            this.database = database;
            Items = new ObservableCollection<Livro>();
            RefreshCommand = new Command(async () => await GetItemsAsync(SearchText));
            SearchCommand = new Command<string>(async (string value) => await GetItemsAsync(value));
        }

        public async Task GetItemsAsync(string searchText)
        {
            IsRefreshing = true;

            Items.Clear();
            var result = await database.GetLivrosAsync(Settings.Usuario?.Id ?? 0);
            if (!string.IsNullOrEmpty(searchText))
            {
                result = result.Where(e => e.Titulo.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                    || e.Autor.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                    || e.ISBN == searchText)
                    .ToList();
            }
            Items.Clear();
            foreach (var item in result.OrderBy(e => e.Titulo))
            {
                Items.Add(item);
            }
            IsRefreshing = false;
        }
    }
}
