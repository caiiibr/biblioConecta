namespace Biblioconecta.Pages;

public partial class InformacaoPage : ContentPage
{
    public InformacaoPage()
    {
        InitializeComponent();
    }

    private async void Entrar_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//TutorialPage");
    }
}