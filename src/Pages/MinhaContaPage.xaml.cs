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
        PasswordEntry.Text = string.Empty;
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

        var usuario = await database.GetUsuarioAsync(Settings.Usuario!.Id);
        usuario!.Nome = NomeEntry.Text;
        usuario!.Email = EmailEntry.Text;
        if (!string.IsNullOrEmpty(PasswordEntry.Text))
        {
            usuario!.Senha = PasswordEntry.Text;
        }
        Settings.Usuario = usuario;
        await database.SaveUsuarioAsync(usuario);
        await Shell.Current.GoToAsync("//PrateleirasPage");
    }
}