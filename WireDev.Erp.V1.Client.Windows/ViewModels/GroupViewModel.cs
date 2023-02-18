using HandyControl.Tools.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WireDev.Erp.V1.Client.Windows.Classes;
using WireDev.Erp.V1.Models.Authentication;
using WireDev.Erp.V1.Models.Storage;

namespace WireDev.Erp.V1.Client.Windows.ViewModels
{
    internal class GroupViewModel : BaseViewModel
    {
        public GroupViewModel()
        {
            Title = "Groups";
        }
        
        private List<int>? _groupsIds = null;
        public List<int> GroupIds
        {
            get => _groupsIds;
            set
            {
                if (_groupsIds != value)
                {
                    _groupsIds = value;
                    base.OnPropertyChanged(nameof(GroupIds));
                }
            }
        }

        private Dictionary<int, Group?>? _groups = null;
        public Dictionary<int, Group?>? Groups
        {
            get => _groups;
            set
            {
                if (_groups != value)
                {
                    _groups = value;
                    base.OnPropertyChanged(nameof(Groups));
                }
            }
        }

        private Group? _selectedProduct = null;
        public Group? SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                if (_selectedProduct != value)
                {
                    _selectedProduct = value;
                    base.OnPropertyChanged(nameof(SelectedProduct));
                }
            }
        }


        private bool CanGetGroupList = true;
        private ICommand? _getGroupListCommand;
        public ICommand GetGroupListCommand => _getGroupListCommand ??= new RelayCommand(param => GetGroupListAsync().ConfigureAwait(true), param => CanGetGroupList);

        private ICommand? _getGroupCommand;
        public ICommand GetGroupCommand => _getGroupCommand ??= new RelayCommand(param => GetGroupAsync((int)param).ConfigureAwait(true));

        public CancellationTokenSource? groupListLoadCts = null;
        public CancellationTokenSource? groupLoadCts = null;


        private async Task<List<int>> GetGroupListAsync()
        {
            CanGetGroupList = false;
            using (groupListLoadCts = new())
            {
                try
                {
                    using HttpResponseMessage response = await ApiConnection.Client.GetAsync($"api/Products/all", groupListLoadCts.Token);
                    Response? r = await response.Content.ReadFromJsonAsync<Response>(cancellationToken: groupListLoadCts.Token);

                    if (response.IsSuccessStatusCode)
                    {
                        return GroupIds = r.Data is List<int> list ? list : throw new ArgumentNullException("Response is not as expected!");
                    }
                    else
                    {
                        throw new Exception("Request was not succesfull!");
                    }
                }
                catch (OperationCanceledException)
                {

                }
                catch (Exception)
                {
                }
                finally { CanGetGroupList = true; }
            }
            return null;
        }

        private async Task<Group> GetGroupAsync(int id)
        {
            using (groupLoadCts = new())
            {
                try
                {
                    using HttpResponseMessage response = await ApiConnection.Client.GetAsync($"api/Groups/{id}", groupLoadCts.Token);
                    Response? r = await response.Content.ReadFromJsonAsync<Response>(cancellationToken: groupLoadCts.Token);
                    if (response.IsSuccessStatusCode && r.Data is Group g)
                    {
                        if (!Groups.ContainsKey(id)) { Groups.Add(id, g); }
                        else { Groups[id] = g; }
                        return g;
                    }
                    else
                    {
                        throw new ArgumentNullException(nameof(GetGroupAsync), "Response is not as expected!");
                    }
                }
                catch (OperationCanceledException ex)
                {

                }
                catch (Exception ex)
                {

                }
            }
            return null;
        }

        private async Task<int> AddGroupAsync(Group group, CancellationToken token)
        {
            try
            {
                using HttpResponseMessage response = await ApiConnection.Client.PutAsJsonAsync($"api/Prices/add", group, token);
                Response? r = await response.Content.ReadFromJsonAsync<Response>(cancellationToken: token);
                if (response.IsSuccessStatusCode && r.Data is Group g)
                {
                    if (!Groups.ContainsKey(g.Uuid)) { Groups.Add(g.Uuid, g); }
                    else { Groups[g.Uuid] = g; }
                    return g.Uuid;
                }
                else
                {
                    throw new ArgumentNullException(nameof(AddGroupAsync), "Response is not as expected!");
                }
            }
            catch (OperationCanceledException ex)
            {

            }
            catch (Exception ex)
            {

            }
            return 0;
        }

        private async Task<Group> UpdateGroupAsync(Group group, CancellationToken token)
        {
            try
            {
                using HttpResponseMessage response = await ApiConnection.Client.PutAsJsonAsync($"api/Groups/{group.Uuid}", group, token);
                Response? r = await response.Content.ReadFromJsonAsync<Response>(cancellationToken: token);
                if (response.IsSuccessStatusCode && r.Data is Group g)
                {
                    if (!Groups.ContainsKey(g.Uuid)) { Groups.Add(g.Uuid, g); }
                    else { Groups[g.Uuid] = g; }
                    return g;
                }
                else
                {
                    throw new ArgumentNullException(nameof(UpdateGroupAsync), "Response is not as expected!");
                }
            }
            catch (OperationCanceledException ex)
            {

            }
            catch (Exception ex)
            {

            }
            return null;
        }

        private async Task<bool> DeleteGroupAsync(int id, CancellationToken token)
        {
            try
            {
                using HttpResponseMessage response = await ApiConnection.Client.PutAsJsonAsync($"api/Groups/{id}", token);
                if (response.IsSuccessStatusCode)
                {
                    if (Groups.ContainsKey(id)) { Groups.Remove(id); }
                    return true;
                }
                else
                {
                    throw new ArgumentNullException(nameof(DeleteGroupAsync), "Response is not as expected!");
                }
            }
            catch (OperationCanceledException ex)
            {

            }
            catch (Exception ex)
            {

            }
            return false;
        }
    }
}
