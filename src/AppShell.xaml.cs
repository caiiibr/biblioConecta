using Biblioconecta.Pages;

namespace Biblioconecta;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute("//LoginPage", typeof(LoginPage));
        Routing.RegisterRoute("//RegistroPage", typeof(RegistroPage));
        Routing.RegisterRoute("//LivroEditPage", typeof(LivroEditPage));
    }

    private async void NovoLivro_Clicked(object sender, EventArgs e)
    {
        await Current.GoToAsync($"//LivroEditPage?id=0");
    }
}
