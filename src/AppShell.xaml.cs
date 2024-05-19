using Biblioconecta.Pages;
using System.Windows.Input;

namespace Biblioconecta
{
    public partial class AppShell : Shell
    {
        ICommand GoBackCommand { get; }

        public AppShell()
        {
            InitializeComponent();

            GoBackCommand = new Command(async () => await Current.GoToAsync(".."));

            Routing.RegisterRoute("//LoginPage", typeof(LoginPage));
            Routing.RegisterRoute("//RegistroPage", typeof(RegistroPage));
        }
    }
}
