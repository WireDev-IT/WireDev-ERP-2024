using HandyControl.Controls;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using WireDev.Erp.V1.Client.Windows.Classes;
using WireDev.Erp.V1.Models.Authentication;

namespace WireDev.Erp.V1.Client.Windows.ViewModels
{
    internal class UserManagementViewModel : BaseViewModel
    {
        public UserManagementViewModel()
        {
            Title = "User management";
        }


        public CancellationTokenSource? addUserCts = null;


        private async Task<bool> AddUserAsync(RegisterModel register)
        {
            using (addUserCts = new())
            {
                try
                {
                    using HttpResponseMessage response = await ApiConnection.Client.PostAsJsonAsync($"api/Prices/add", register, addUserCts.Token);
                    Response? r = await response.Content.ReadFromJsonAsync<Response>(cancellationToken: addUserCts.Token);
                    if (response.IsSuccessStatusCode)
                    {
                        _ = MessageBox.Success(r.Message, "Creating user");
                        return true;
                    }
                    else
                    {
                        throw new ArgumentNullException(nameof(AddUserAsync), r.Message);
                    }
                }
                catch (OperationCanceledException ex)
                {
                    _ = MessageBox.Warning(ex.Message, "Creating user");
                }
                catch (Exception ex)
                {
                    _ = MessageBox.Error(ex.Message, "Creating user");
                }
            }
            return false;
        }
    }
}