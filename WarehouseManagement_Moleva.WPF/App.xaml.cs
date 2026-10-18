using System.Windows;

namespace WarehouseManagement_Moleva.WPF
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var window = new OrganizationsWindow();
            window.Show();
        }
    }
}