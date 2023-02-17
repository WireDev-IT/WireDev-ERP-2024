using HandyControl.Controls;
using HandyControl.Tools.Extension;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows;
using WireDev.Erp.V1.Client.Windows.Classes;
using ConfigurationManager = WireDev.Erp.V1.Client.Windows.Classes.ConfigurationManager;
using MessageBox = HandyControl.Controls.MessageBox;

namespace WireDev.Erp.V1.Client.Windows
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            StartupWindow window = new(await LoadConfig());
            window.Show();
        }

        private async Task<ConfigurationManager> LoadConfig()
        {
            ConfigurationManager? config = null;
            try
            {
                //return false;
                //TODO: Load file
                ApiConnection.SetClient(new Uri("https://127.0.0.1:7216/"), new MediaTypeWithQualityHeaderValue("application/json"));

                //TODO: Localization
                CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
                CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");
            }
            catch (Exception ex)
            {
                MessageBox.Error(ex.Message, "Loading configuration failed");
            }

            return config;
        }
    }
}
