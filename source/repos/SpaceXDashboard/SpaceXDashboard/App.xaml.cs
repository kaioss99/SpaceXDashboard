using PdfSharp.Fonts;
using SpaceXDashboard.Commands;
using SpaceXDashboard.Data;

using System.Windows;

namespace SpaceXDashboard
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            GlobalFontSettings.FontResolver = new FileFontResolver();

            var db = new DatabaseContext();
            db.Initialize();
        }
    }
}