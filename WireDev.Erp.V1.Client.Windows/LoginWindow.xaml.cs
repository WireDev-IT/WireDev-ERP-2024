using HandyControl.Controls;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using WireDev.Erp.V1.Models.Authentication;
using MessageBox = HandyControl.Controls.MessageBox;
using Window = System.Windows.Window;

namespace WireDev.Erp.V1.Client.Windows
{
    /// <summary>
    /// Interaktionslogik für LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly AnimationPath LoadingAnimation;
        private readonly ConnectionManager connection;

        public LoginWindow()
        {
            InitializeComponent();
            LoadingAnimation = new()
            {
                Data = Geometry.Parse(
                    "M 175.5,-0.5 C 175.833,-0.5 176.167,-0.5 176.5,-0.5C 177.167,0.833333 177.833,0.833333 178.5,-0.5C 178.833,-0.5 179.167,-0.5 179.5,-0.5C 180.167,0.833333 180.833,0.833333 181.5,-0.5C 181.833,-0.5 182.167,-0.5 182.5,-0.5C 183.167,0.833333 183.833,0.833333 184.5,-0.5C 184.833,-0.5 185.167,-0.5 185.5,-0.5C 186.167,0.833333 186.833,0.833333 187.5,-0.5C 187.833,-0.5 188.167,-0.5 188.5,-0.5C 189.167,0.833333 189.833,0.833333 190.5,-0.5C 190.833,-0.5 191.167,-0.5 191.5,-0.5C 192.167,0.833333 192.833,0.833333 193.5,-0.5C 193.833,-0.5 194.167,-0.5 194.5,-0.5C 195.167,0.833333 195.833,0.833333 196.5,-0.5C 196.833,-0.5 197.167,-0.5 197.5,-0.5C 198.167,0.833333 198.833,0.833333 199.5,-0.5C 199.833,-0.5 200.167,-0.5 200.5,-0.5C 201.167,0.833333 201.833,0.833333 202.5,-0.5C 202.833,-0.5 203.167,-0.5 203.5,-0.5C 204.167,0.833333 204.833,0.833333 205.5,-0.5C 205.833,-0.5 206.167,-0.5 206.5,-0.5C 207.167,0.833333 207.833,0.833333 208.5,-0.5C 208.833,-0.5 209.167,-0.5 209.5,-0.5C 210.167,0.833333 210.833,0.833333 211.5,-0.5C 211.833,-0.5 212.167,-0.5 212.5,-0.5C 213.167,0.833333 213.833,0.833333 214.5,-0.5C 214.833,-0.5 215.167,-0.5 215.5,-0.5C 216.167,0.833333 216.833,0.833333 217.5,-0.5C 217.833,-0.5 218.167,-0.5 218.5,-0.5C 219.167,0.833333 219.833,0.833333 220.5,-0.5C 220.833,-0.5 221.167,-0.5 221.5,-0.5C 222.167,0.833333 222.833,0.833333 223.5,-0.5C 223.833,-0.5 224.167,-0.5 224.5,-0.5C 225.167,0.833333 225.833,0.833333 226.5,-0.5C 226.833,-0.5 227.167,-0.5 227.5,-0.5C 228.167,0.833333 228.833,0.833333 229.5,-0.5C 229.833,-0.5 230.167,-0.5 230.5,-0.5C 231.167,0.833333 231.833,0.833333 232.5,-0.5C 232.833,-0.5 233.167,-0.5 233.5,-0.5C 234.167,0.833333 234.833,0.833333 235.5,-0.5C 235.833,-0.5 236.167,-0.5 236.5,-0.5C 237.167,0.833333 237.833,0.833333 238.5,-0.5C 238.833,-0.5 239.167,-0.5 239.5,-0.5C 240.167,0.833333 240.833,0.833333 241.5,-0.5C 241.833,-0.5 242.167,-0.5 242.5,-0.5C 243.167,0.833333 243.833,0.833333 244.5,-0.5C 244.833,-0.5 245.167,-0.5 245.5,-0.5C 246.167,0.833333 246.833,0.833333 247.5,-0.5C 247.833,-0.5 248.167,-0.5 248.5,-0.5C 249.167,0.833333 249.833,0.833333 250.5,-0.5C 250.833,-0.5 251.167,-0.5 251.5,-0.5C 252.167,0.833333 252.833,0.833333 253.5,-0.5C 253.833,-0.5 254.167,-0.5 254.5,-0.5C 255.167,0.833333 255.833,0.833333 256.5,-0.5C 256.833,-0.5 257.167,-0.5 257.5,-0.5C 258.167,0.833333 258.833,0.833333 259.5,-0.5C 259.833,-0.5 260.167,-0.5 260.5,-0.5C 261.167,0.833333 261.833,0.833333 262.5,-0.5C 262.833,-0.5 263.167,-0.5 263.5,-0.5C 264.167,0.833333 264.833,0.833333 265.5,-0.5C 265.833,-0.5 266.167,-0.5 266.5,-0.5C 267.167,0.833333 267.833,0.833333 268.5,-0.5C 268.833,-0.5 269.167,-0.5 269.5,-0.5C 270.167,0.833333 270.833,0.833333 271.5,-0.5C 271.833,-0.5 272.167,-0.5 272.5,-0.5C 273.167,0.833333 273.833,0.833333 274.5,-0.5C 274.833,-0.5 275.167,-0.5 275.5,-0.5C 276.167,0.833333 276.833,0.833333 277.5,-0.5C 277.833,-0.5 278.167,-0.5 278.5,-0.5C 279.167,0.833333 279.833,0.833333 280.5,-0.5C 280.833,-0.5 281.167,-0.5 281.5,-0.5C 282.167,0.833333 282.833,0.833333 283.5,-0.5C 283.833,-0.5 284.167,-0.5 284.5,-0.5C 285.167,0.833333 285.833,0.833333 286.5,-0.5C 286.833,-0.5 287.167,-0.5 287.5,-0.5C 288.167,0.833333 288.833,0.833333 289.5,-0.5C 289.833,-0.5 290.167,-0.5 290.5,-0.5C 291.167,0.833333 291.833,0.833333 292.5,-0.5C 292.833,-0.5 293.167,-0.5 293.5,-0.5C 294.167,0.833333 294.833,0.833333 295.5,-0.5C 295.833,-0.5 296.167,-0.5 296.5,-0.5C 297.167,0.833333 297.833,0.833333 298.5,-0.5C 298.833,-0.5 299.167,-0.5 299.5,-0.5C 300.167,0.833333 300.833,0.833333 301.5,-0.5C 301.833,-0.5 302.167,-0.5 302.5,-0.5C 303.167,0.833333 303.833,0.833333 304.5,-0.5C 304.833,-0.5 305.167,-0.5 305.5,-0.5C 306.167,0.833333 306.833,0.833333 307.5,-0.5C 307.833,-0.5 308.167,-0.5 308.5,-0.5C 308.918,0.221579 309.584,0.721579 310.5,1C 287.599,23.7359 264.599,46.2359 241.5,68.5C 218.115,46.4585 195.115,23.9585 172.5,1C 173.737,0.76791 174.737,0.26791 175.5,-0.5 Z " +
                    "M 483.5,403.5 C 458.833,403.5 434.167,403.5 409.5,403.5C 408.336,293.903 408.169,184.236 409,74.5C 433.667,49.8333 458.333,25.1667 483,0.5C 483.5,134.833 483.667,269.166 483.5,403.5 Z " +
                    "M -0.5,0.5 C 24.4653,24.9651 49.2986,49.6318 74,74.5C 74.6667,176.167 74.6667,277.833 74,379.5C 65.9642,387.368 58.1309,395.368 50.5,403.5C 33.5,403.5 16.5,403.5 -0.5,403.5C -0.5,269.167 -0.5,134.833 -0.5,0.5 Z " +
                    "M 101.5,104.5 C 125.827,128.66 150.327,152.66 175,176.5C 176.151,191.077 176.651,205.744 176.5,220.5C 176.333,234.5 176.167,248.5 176,262.5C 151.333,287.167 126.667,311.833 102,336.5C 101.5,259.167 101.333,181.834 101.5,104.5 Z " +
                    "M 381.5,403.5 C 356.833,403.5 332.167,403.5 307.5,403.5C 306.337,333.237 306.17,262.904 307,192.5C 331.474,167.938 356.14,143.605 381,119.5C 381.5,214.166 381.667,308.833 381.5,403.5 Z " +
                    "M 203.5,192.5 C 211.298,199.798 218.965,207.298 226.5,215C 219,222.5 211.5,230 204,237.5C 203.5,222.504 203.333,207.504 203.5,192.5 Z " +
                    "M 279.5,403.5 C 279.167,403.5 278.833,403.5 278.5,403.5C 278.167,402.167 277.833,402.167 277.5,403.5C 277.167,403.5 276.833,403.5 276.5,403.5C 275.833,402.167 275.167,402.167 274.5,403.5C 274.167,403.5 273.833,403.5 273.5,403.5C 272.833,402.167 272.167,402.167 271.5,403.5C 271.167,403.5 270.833,403.5 270.5,403.5C 269.833,402.167 269.167,402.167 268.5,403.5C 268.167,403.5 267.833,403.5 267.5,403.5C 266.833,402.167 266.167,402.167 265.5,403.5C 265.167,403.5 264.833,403.5 264.5,403.5C 263.833,402.167 263.167,402.167 262.5,403.5C 262.167,403.5 261.833,403.5 261.5,403.5C 260.833,402.167 260.167,402.167 259.5,403.5C 259.167,403.5 258.833,403.5 258.5,403.5C 257.833,402.167 257.167,402.167 256.5,403.5C 256.167,403.5 255.833,403.5 255.5,403.5C 254.833,402.167 254.167,402.167 253.5,403.5C 253.167,403.5 252.833,403.5 252.5,403.5C 251.833,402.167 251.167,402.167 250.5,403.5C 250.167,403.5 249.833,403.5 249.5,403.5C 248.833,402.167 248.167,402.167 247.5,403.5C 247.167,403.5 246.833,403.5 246.5,403.5C 245.833,402.167 245.167,402.167 244.5,403.5C 244.167,403.5 243.833,403.5 243.5,403.5C 242.833,402.167 242.167,402.167 241.5,403.5C 241.167,403.5 240.833,403.5 240.5,403.5C 239.833,402.167 239.167,402.167 238.5,403.5C 238.167,403.5 237.833,403.5 237.5,403.5C 236.833,402.167 236.167,402.167 235.5,403.5C 235.167,403.5 234.833,403.5 234.5,403.5C 233.833,402.167 233.167,402.167 232.5,403.5C 232.167,403.5 231.833,403.5 231.5,403.5C 230.833,402.167 230.167,402.167 229.5,403.5C 229.167,403.5 228.833,403.5 228.5,403.5C 227.833,402.167 227.167,402.167 226.5,403.5C 226.167,403.5 225.833,403.5 225.5,403.5C 224.833,402.167 224.167,402.167 223.5,403.5C 223.167,403.5 222.833,403.5 222.5,403.5C 221.833,402.167 221.167,402.167 220.5,403.5C 220.167,403.5 219.833,403.5 219.5,403.5C 218.833,402.167 218.167,402.167 217.5,403.5C 217.167,403.5 216.833,403.5 216.5,403.5C 215.833,402.167 215.167,402.167 214.5,403.5C 214.167,403.5 213.833,403.5 213.5,403.5C 212.833,402.167 212.167,402.167 211.5,403.5C 211.167,403.5 210.833,403.5 210.5,403.5C 209.833,402.167 209.167,402.167 208.5,403.5C 208.167,403.5 207.833,403.5 207.5,403.5C 206.602,402.842 205.602,402.176 204.5,401.5C 205.081,360.876 205.081,320.043 204.5,279C 229.333,254.167 254.167,229.333 279,204.5C 279.5,270.832 279.667,337.166 279.5,403.5 Z " +
                    "M 177.5,402.5 C 144.663,402.063 112.329,402.063 80.5,402.5C 112.465,370.035 144.632,337.701 177,305.5C 177.5,337.832 177.667,370.165 177.5,402.5 Z"),
                Duration = new Duration(TimeSpan.FromSeconds(4)),
                Height = 100,
                Stroke = (Brush?)new BrushConverter().ConvertFromString("#FF59606D"),
                StrokeThickness = 2,
                IsPlaying = true,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            connection = new(new Uri("https://127.0.0.1:7216/"), new MediaTypeWithQualityHeaderValue("application/json"));
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

        private async void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            Storyboard? sb = FindResource("LoginSubmit") as Storyboard;
            sb.Begin();
            _ = FormPanel.Children.Add(LoadingAnimation);
            await SetSubtitel("Connecting to server...");

            if (!await connection.IsOnline())
            {
                await SetSubtitel("Connection error");
                _ = MessageBox.Show("The server refused to connect! We can not log you in. Try again later.", "Connection error", MessageBoxButton.OK, MessageBoxImage.Error);
                
                Storyboard? sb2 = FindResource("LoginReset") as Storyboard;
                sb2.Begin();
                FormPanel.Children.Remove(LoadingAnimation);
                await SetSubtitel("Login into your Account");
            }
            else
            {
                await SetSubtitel("Loggin in...");
                try
                {
                    HttpResponseMessage response = await connection.Client.PostAsJsonAsync<LoginModel>("api/Authenticate/login",
                        new() { Username = UsernameInput.Password, Password = PasswordInput.Password });
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        connection.SetToken((await response.Content.ReadFromJsonAsync<Response>()).Data.ToString());
                    }
                    else
                    {
                        _ = MessageBox.Show("Either username or password is incorrect!", "Authentication", MessageBoxButton.OK, MessageBoxImage.Error);
                        UsernameInput.IsError = true;
                        PasswordInput.IsError = true;
                    }
                }
                catch (HttpRequestException ex)
                {
                    _ = MessageBox.Show("It was not possible to send your credentials to the server for validation. Try again later.", "Connection error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    _ = MessageBox.Show($"An unexpected error occured: {ex.Message}\n\nContact your administrator if this happens more than two times.", "Unknown error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                SubtitleTxt.Text = "Retrieving data...";
                MainWindow window = new(connection);
                window.Show();
                Close();
            }
        }

        private void TroubleBtn_Click(object sender, RoutedEventArgs e)
        {
            _ = MessageBox.Show("Send a request to your administrator to regain access to your account.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void UsernameInput_TextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            UsernameInput.IsError = false;
        }

        private void PasswordInput_TextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            PasswordInput.IsError = false;
        }

        private Task SetSubtitel(string text)
        {
            SubtitelTransition.Visibility = Visibility.Collapsed;
            SubtitleTxt.Text = text;
            SubtitelTransition.Visibility = Visibility.Visible;
            return Task.CompletedTask;
        }
    }
}