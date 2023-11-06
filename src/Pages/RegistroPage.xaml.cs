using Biblioconecta.Data;
using Biblioconecta.Data.Models;

namespace Biblioconecta.Pages;

public partial class RegistroPage : ContentPage
{
    private readonly BiblioconectaDatabase database;

    public RegistroPage(BiblioconectaDatabase database)
    {
        InitializeComponent();
        this.database = database;
    }

    private async void Registrar_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(NomeEntry.Text))
        {
            await Shell.Current.DisplayAlert("Algo deu errado...", "O nome � obrigat�rio.", "Ok, entendi");
            return;
        }
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
        if (SenhaEntry.Text != ConfirmacaoSenhaEntry.Text)
        {
            await Shell.Current.DisplayAlert("Algo deu errado...", "A confirma��o da senha deve ser igual a senha.", "Ok, entendi");
            return;
        }
        if (await database.UsuarioExists(EmailEntry.Text))
        {
            await Shell.Current.DisplayAlert("Algo deu errado...", "O e-mail informado j� foi usado anteriormente.", "Ok, entendi");
            return;
        }

        Usuario usuario = new()
        {
            Nome = NomeEntry.Text,
            Email = EmailEntry.Text,
            Senha = SenhaEntry.Text
        };
        await database.SaveUsuarioAsync(usuario);

        await Shell.Current.DisplayAlert("Deu tudo certo...", "A sua conta foi criada com sucesso!", "Ok, entendi");
        await Shell.Current.GoToAsync("//LoginPage");
    }

#if DEBUG
    protected override void OnAppearing()
    {
        base.OnAppearing();
        NomeEntry.Text = "Usuário";
        EmailEntry.Text = "usuario@gmail.com";
        SenhaEntry.Text = "123";
        ConfirmacaoSenhaEntry.Text = SenhaEntry.Text;
    }
#endif
}