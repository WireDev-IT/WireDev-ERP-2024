using HandyControl.Controls;
using HandyControl.Tools.Command;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WireDev.Erp.V1.Client.Windows.Classes;
using WireDev.Erp.V1.Models.Authentication;
using WireDev.Erp.V1.Models.Enums;
using WireDev.Erp.V1.Models.Storage;

namespace WireDev.Erp.V1.Client.Windows.ViewModels
{
    internal class CheckoutViewModel : BaseViewModel
    {
        public CheckoutViewModel()
        {

        }

        public TransactionType ProcedureType { get; set; }
        public List<TransactionItem> CheckoutItems { get; set; } = new();
        private Purchase? purchaseToPush = null;

        private readonly Dictionary<uint, CancellationTokenSource> tokens = new();
        private CancellationTokenSource? purchaseCts = null;

        public ICommand GetProductCommand => new RelayCommand(param => GetProductAsync((uint)param).ConfigureAwait(true));
        public ICommand SellCommand => new RelayCommand(param => SellTransaction().ConfigureAwait(true));

        private async Task<Product> GetProductAsync(uint id)
        {
            if (tokens.ContainsKey(id))
            {
                try
                {
                    using CancellationTokenSource cts = new();
                    tokens.Add(id, cts);
                    using HttpResponseMessage response = await ApiConnection.Client.GetAsync($"api/Products/{id}", cts.Token);
                    Response? r = await response.Content.ReadFromJsonAsync<Response>();
                    return response.IsSuccessStatusCode && (response.Value is Product p) ? p : throw new ArgumentNullException(nameof(GetProductAsync), "Response is not as expected!");
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

        private async Task SellTransaction()
        {
            foreach (TransactionItem item in CheckoutItems)
            {
                purchaseToPush.Items.Add(item);
            }

            try
            {
                using (purchaseCts = new())
                {
                    using HttpResponseMessage response = await ApiConnection.Client.PostAsJsonAsync("api/Transactions/sell", purchaseToPush, purchaseCts.Token);
                    if (response.IsSuccessStatusCode)
                    {
                        if (await response.Content.ReadFromJsonAsync<Response>() is Response r)
                        {
                            purchaseToPush = null;
                            CheckoutItems.Clear();

                            if (response.Value is Guid g)
                            {
                                _ = MessageBox.Success("Transaction done!", "Sell");
                                CheckoutItems.Clear();
                            }
                            else
                            {
                                _ = MessageBox.Error("Transaction done! Unable to get transaction id.", "Sell");
                            }
                        }
                    }
                    else
                    {
                        _ = MessageBox.Error("Transaction failed!", "Sell");
                    }
                }
            }
            catch (OperationCanceledException)
            {

            }
            catch (Exception)
            {

            }
        }
    }
}