using HandyControl.Tools.Command;
using HandyControl.Tools.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WireDev.Erp.V1.Client.Windows.Classes;
using WireDev.Erp.V1.Models.Authentication;
using WireDev.Erp.V1.Models.Statistics;
using WireDev.Erp.V1.Models.Storage;

namespace WireDev.Erp.V1.Client.Windows.ViewModels
{
    internal class TransactionsViewModel : BaseViewModel
    {
        public TransactionsViewModel()
        {
            Title = "Transactions";
        }

        private Dictionary<Guid, Purchase?> _purchaseList = new();
        public Dictionary<Guid, Purchase?> PurchaseList
        {
            get => _purchaseList;
            set
            {
                if (_purchaseList != value)
                {
                    _purchaseList = value;
                    base.OnPropertyChanged(nameof(PurchaseList));
                }
            }
        }

        private readonly Dictionary<Guid, CancellationTokenSource> tokens = new();
        private CancellationTokenSource? listCts = null;
        private CancellationTokenSource? bulkLoadCts = null;

        private bool CanBulkLoad = true;
        public ICommand BulkLoadCommand => new RelayCommand(param => BulkLoadPurchases((bool)param).ConfigureAwait(true), param => CanBulkLoad);

        private bool CanGetPurchaseList = true;
        public ICommand GetPurchaseListCommand => new RelayCommand(param => GetPurchaseListAsync().ConfigureAwait(true), param => CanGetPurchaseList);

        private async Task<bool> GetPurchaseListAsync()
        {
            CanGetPurchaseList = false;
            using (listCts = new())
            {
                try
                {
                    using HttpResponseMessage response = await ApiConnection.Client.GetAsync($"api/Purchase/all", listCts.Token);
                    Response? r = await response.Content.ReadFromJsonAsync<Response>();
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.Value is List<Guid> list)
                        {
                            listCts.Token.ThrowIfCancellationRequested();
                            PurchaseList.Clear();

                            foreach (var item in list)
                            {
                                PurchaseList.Add(item, null);
                                listCts.Token.ThrowIfCancellationRequested();
                            }

                            CanGetPurchaseList = true;
                            return true;
                        }
                    }
                    else
                    {

                    }
                }
                catch (OperationCanceledException)
                {

                }
                catch (Exception)
                {

                }
            }
            CanGetPurchaseList = true;
            return false;
        }

        private async Task<Purchase> GetPurchaseAsync(Guid id)
        {
            if (tokens.ContainsKey(id))
            {
                try
                {
                    using CancellationTokenSource cts = new();
                    tokens.Add(id, cts);
                    using HttpResponseMessage response = await ApiConnection.Client.GetAsync($"api/Purchase/{id}", cts.Token);
                    Response? r = await response.Content.ReadFromJsonAsync<Response>();
                    return response.IsSuccessStatusCode && (response.Value is Purchase p) ? p : throw new ArgumentNullException(nameof(GetPurchaseAsync), "Response is not as expected!");
                }
                catch (ArgumentNullException)
                {

                }
                catch (Exception)
                {

                }
                finally
                {
                    _ = tokens.Remove(id);
                }
            }
            return null;
        }

        private async Task BulkLoadPurchases(bool forceReload = false)
        {
            CanBulkLoad = false;
            if (PurchaseList != null && PurchaseList.Count > 0)
            {
                using (bulkLoadCts = new())
                {
                    foreach (Guid id in PurchaseList.Keys)
                    {
                        if (forceReload || PurchaseList.GetValueOrDefault(id) == null)
                        {
                            if (await GetPurchaseAsync(id) is Purchase p)
                            {
                                PurchaseList.Update(id, p);
                            }
                        }
                    }
                }
            }
            CanBulkLoad = true;
        }
    }
}