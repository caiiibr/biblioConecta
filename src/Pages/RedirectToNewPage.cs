namespace Biblioconecta.Pages;

public class RedirectToNewPage : ContentPage
{
    public RedirectToNewPage()
    {
        Content = new VerticalStackLayout
        {
            //Children = {
            //	new Label { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Text = "Welcome to .NET MAUI!"
            //	}
            //}
        };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Shell.Current.GoToAsync("//LivroEditPage?id=0&openCamera=False");
    }
}