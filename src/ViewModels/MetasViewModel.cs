using Biblioconecta.Data;
using Biblioconecta.Data.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Biblioconecta.ViewModels
{
    public class MetasViewModel : BaseViewModel
    {
        private bool isRefreshing = true;
        private readonly BiblioconectaDatabase database;

        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand RefreshCommand { get; }

        public ObservableCollection<Meta> Items { get; } = new();
        public bool IsRefreshing { get => isRefreshing; set => SetProperty(ref isRefreshing, value); }

        public MetasViewModel(BiblioconectaDatabase database)
        {
            this.database = database;
            DeleteCommand = new Command<Meta>(async (value) => await DeleteItemAsync(value));
            EditCommand = new Command<Meta?>(async (value) => await EditItemAsync(value));
            RefreshCommand = new Command(async () => await GetItemsAsync());
        }

        public async Task GetItemsAsync()
        {
            IsRefreshing = true;

            var result = await database.GetMetasAsync(Settings.Usuario?.Id ?? 0);
            Items.Clear();
            foreach (var item in result.OrderBy(e => e.Nome))
            {
                Items.Add(item);
            }

            IsRefreshing = false;
        }

        public async Task EditItemAsync(Meta? value)
        {
            await Shell.Current.GoToAsync($"//MetaEditPage?id={value?.Id ?? 0}");
        }

        public async Task DeleteItemAsync(Meta value)
        {
            var result = await Shell.Current.DisplayAlert("Remover?", "Deseja realmente remover a meta?", "Sim, eu quero", "Cancelar");
            if (result)
            {
                await database.DeleteMetaAsync(value);
                await GetItemsAsync();
            }
        }
    }
}
