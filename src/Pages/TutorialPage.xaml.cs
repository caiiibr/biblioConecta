using Biblioconecta.Models;
using System.Collections.ObjectModel;

namespace Biblioconecta.Pages;

public partial class TutorialPage : ContentPage
{
    public ObservableCollection<TutorialItem> Items { get; private set; }

    public TutorialPage()
    {
        InitializeComponent();

        Items = new ObservableCollection<TutorialItem>
        {
            new() { Title = "Vamos come�ar!", Description = $"Pegue um livro que queira catalogar.\r\nLivro em m�os?\r\nCrie uma ou mais estantes na aba 'Prateleiras'.", Image = "nova_prateleira.png", ImageFirst = true },
            new() { Title = "Adicione Livros!", Description = "Insira livros manualmente ou escaneie o c�digo de barras no verso do livro.", Image = "cadastro_livros.png", ImageFirst = false },
            new() { Title = "Trace metas de leitura!", Description = "Estabele�a objetivos de acordo com seu tempo livre dispon�vel, para otimizar o uso do seu tempo e melhorar sua produtividade.", Image = "metas.png", ImageFirst = true },
            new() { Title = "Compartilhe listas de leitura!", Description = "Professores podem compartilhar listas de livros selecionados diretamente para os dispositivos dos alunos,simplificando o acesso �s leituras obrigat�rias.", Image = "compatilhar.png", ImageFirst = false },
        };

        carouselView.ItemsSource = Items;
        //carouselView.SetBinding(ItemsView.ItemsSourceProperty, "Items");
        //carouselView.SetBinding(CarouselView.PositionProperty, "Position");
    }

    private async void Entrar_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//PrateleirasPage");
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//PrateleirasPage");
    }

    private void carouselView_PositionChanged(object sender, PositionChangedEventArgs e)
    {
        pularLabel.IsVisible = e.CurrentPosition < 3;
        entrarButton.IsVisible = e.CurrentPosition == 3;
    }
}