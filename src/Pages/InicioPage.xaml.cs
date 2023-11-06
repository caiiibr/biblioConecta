namespace Biblioconecta.Pages;

public partial class InicioPage : ContentPage
{
    public InicioPage()
    {
        InitializeComponent();
    }

    private async void Registrar_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//RegistroPage");
    }

    private async void Entrar_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//LoginPage");
    }
}