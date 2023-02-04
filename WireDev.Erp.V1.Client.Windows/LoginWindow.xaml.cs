using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WireDev.Erp.V1.Client.Windows
{
    /// <summary>
    /// Interaktionslogik für LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void CredentialsPanel_GotFocus(object sender, RoutedEventArgs e)
        {
            Storyboard? sb = FindResource("LoginPanelFocus") as Storyboard;
            sb.Begin();
        }

        private void CredentialsPanel_LostFocus(object sender, RoutedEventArgs e)
        {
            Storyboard? sb = FindResource("LoginPanelDefocus") as Storyboard;
            sb.Begin();
        }
    }
}
