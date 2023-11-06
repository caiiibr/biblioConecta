using Biblioconecta.Data;
using Biblioconecta.Data.Models;

namespace Biblioconecta.Pages;

public partial class LoginPage : ContentPage
{
    private readonly BiblioconectaDatabase database;

    public LoginPage(BiblioconectaDatabase database)
    {
        InitializeComponent();
        this.database = database;
    }

    private async void Entrar_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(EmailEntry.Text))
        {
            await Shell.Current.DisplayAlert("Algo deu errado...", "O e-mail � obrigat�rio.", "Ok, entendi");
            return;
        }
        if (string.IsNullOrEmpty(SenhaEntry.Text))
        {
            await Shell.Current.DisplayAlert("Algo deu errado...", "A senha � obrigat�ria.", "Ok, entendi");
            return;
        }

        Usuario? usuario = await database.GetUsuarioAsync(EmailEntry.Text);
        if (usuario == null || usuario.Senha != SenhaEntry.Text)
        {
            await Shell.Current.DisplayAlert("Algo deu errado...", "O e-mail ou a senha informado � inv�lido.", "Ok, entendi");
            return;
        }

        await Shell.Current.GoToAsync("//PrateleirasPage");
    }

#if DEBUG
    protected override void OnAppearing()
    {
        base.OnAppearing();
        EmailEntry.Text = "usuario@gmail.com";
        SenhaEntry.Text = "123";
    }
#endif
}