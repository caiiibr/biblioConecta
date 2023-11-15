using Biblioconecta.Data;

namespace Biblioconecta.Pages;

public partial class MinhaContaPage : ContentPage
{
    private readonly BiblioconectaDatabase database;

    public MinhaContaPage(BiblioconectaDatabase database)
	{
		InitializeComponent();
        this.database = database;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        NomeEntry.Text = Settings.Usuario?.Nome;
        EmailEntry.Text = Settings.Usuario?.Email;
    }

    private async void Desconectar_Clicked(object sender, EventArgs e)
    {
        Settings.Clear();
        await Shell.Current.GoToAsync("//InicioPage");
    }

    private async void Salvar_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(NomeEntry.Text))
        {
            await Shell.Current.DisplayAlert("Algo deu errado...", "O Nome é obrigatório.", "Ok, entendi");
            return;
        }

        if (string.IsNullOrEmpty(EmailEntry.Text))
        {
            await Shell.Current.DisplayAlert("Algo deu errado...", "O e-mail é obrigatório.", "Ok, entendi");
            return;
        }

        Settings.Usuario!.Nome = NomeEntry.Text;
        Settings.Usuario!.Email = EmailEntry.Text;
        if (!string.IsNullOrEmpty(PasswordEntry.Text))
        {
            Settings.Usuario!.Senha = PasswordEntry.Text;
        }
        await database.SaveUsuarioAsync(Settings.Usuario);
        await Shell.Current.GoToAsync("//PrateleirasPage");
    }
}