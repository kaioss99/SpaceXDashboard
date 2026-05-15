using System.Windows;
using SpaceXDashboard.Data;
using SpaceXDashboard.Views;

namespace SpaceXDashboard
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Inicializa o banco de dados
            var db = new DatabaseContext();
            db.Initialize();

            // Abre a janela principal
            var mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}