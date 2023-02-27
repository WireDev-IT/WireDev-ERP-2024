using System.Windows;
using WireDev.Erp.V1.Client.Windows.Classes;
using Window = System.Windows.Window;

namespace WireDev.Erp.V1.Client.Windows
{
    /// <summary>
    /// Interaktionslogik für StartupWindow.xaml
    /// </summary>
    public partial class StartupWindow : Window
    {
        private readonly ConfigurationManager? _config;

        public StartupWindow(ConfigurationManager? config = null)
        {
            InitializeComponent();
            _config = config;
        }

        private void Window_Loaded(object? sender, RoutedEventArgs? e)
        {
            if (_config != null)
            {
                LoginWindow login = new();
                login.Show();
                login.WindowState = WindowState.Normal;
                Close();
            }
            else
            {
                //WindowState = WindowState.Normal;
                //ShowInTaskbar = true;

                MainWindow window = new();
                window.Show();
                Close();
            }
        }
    }
}