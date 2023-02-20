using HandyControl.Controls;
using HandyControl.Tools.Command;
using HandyControl.Tools.Extension;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WireDev.Erp.V1.Client.Windows.Classes;
using WireDev.Erp.V1.Models.Authentication;
using WireDev.Erp.V1.Models.Interfaces;
using WireDev.Erp.V1.Models.Storage;

namespace WireDev.Erp.V1.Client.Windows.ViewModels
{
    internal class ProductsViewModel : BaseViewModel
    {
        private List<uint>? _productIds = null;
        public List<uint> ProductIds
        {
            get => _productIds;
            set
            {
                if (_productIds != value)
                {
                    _productIds = value;
                    base.OnPropertyChanged(nameof(ProductIds));
                }
            }
        }

        private Dictionary<uint, DisplayProduct?>? _products = null;
        public Dictionary<uint, DisplayProduct?>? Products
        {
            get => _products;
            set
            {
                if (_products != value)
                {
                    _products = value;
                    base.OnPropertyChanged(nameof(Products));
                }
            }
        }

        private DisplayProduct? _selectedProduct = null;
        public DisplayProduct? SelectedProduct
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

        public ProductsViewModel()
        {
            Title = "Products";
            Products = new()
            {
            {
                    9999,
                    new DisplayProduct(9999)
                    {
                        Active = false,
                        Name = "Test_Product",
                        Archived = true,
                        Description = "Ich bin ein Produkt.",
                        EAN = new() { 4387534643630, 3498573876835 },
                        Prices = new() { Guid.NewGuid() },
                        Properties = new() { { "Attribut", "Wert" } },
                        Group = 99,
                        Metadata= new() { { "Attribut", "Wert" } },
                        DisplayPrices = new() {new DisplayPrice(Guid.NewGuid()) { RetailValue = 5, SellValue = 10, Description = "I bims eins Preis" } }
                    }
            }
            };
            SelectedProduct = Products.Values.ToList()[0];
        }

        private bool CanGetProducts = true;
        private ICommand? _getProductsCommand;
        public ICommand GetProductsCommand => _getProductsCommand ??= new RelayCommand(param => GetProductsAsync().ConfigureAwait(true), param => CanGetProducts);

        private readonly bool CanGetProductsData = true;
        private ICommand? _getProductsDataCommand;
        public ICommand GetProductsDataCommand => _getProductsDataCommand ??= new RelayCommand(param => GetProductsDataAsync().ConfigureAwait(true), param => CanGetProductsData);

        public ICommand AddEan => new RelayCommand(param => SelectedProduct.EAN.Add(0));

        public CancellationTokenSource? priceLoadCts = null;

        private Task StatusCodeHandler(HttpStatusCode code, string caption)
        {
            _ = code switch
            {
                HttpStatusCode.BadRequest => MessageBox.Error("Internal Error: The server expected something else.", caption),
                HttpStatusCode.Forbidden => MessageBox.Error("This action is not allowed", caption),
                HttpStatusCode.NotFound => MessageBox.Warning("We could not find what you are looking for.", caption),
                HttpStatusCode.Unauthorized => MessageBox.Error("You are not allowed to do that!", caption),
                _ => MessageBox.Error("An unknown error occured. Contact your administrator.", caption),
            };
            return Task.CompletedTask;
        }

        private async Task<List<uint>> GetProductsAsync()
        {
            CanGetProducts = false;

            try
            {
                using HttpResponseMessage response = await ApiConnection.Client.GetAsync($"api/Products/all");
                Response? r = await response.Content.ReadFromJsonAsync<Response>();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return ProductIds = r.Data is List<uint> list ? list : throw new ArgumentNullException("Response is not as expected!");
                }
                else
                {
                    await StatusCodeHandler(response.StatusCode, "Retrieving list of products");
                    throw new Exception("Request was not succesfull!");
                }
            }
            catch (Exception)
            {
                return null;
            }
            finally { CanGetProducts = true; }
        }

        private async Task<DisplayProduct> GetProductAsync(uint id)
        {
            using HttpResponseMessage response = await ApiConnection.Client.GetAsync($"api/Products/{id}");
            Response? r = await response.Content.ReadFromJsonAsync<Response>();
            return response.IsSuccessStatusCode && r.Data is DisplayProduct p ? p : throw new ArgumentNullException(nameof(GetProductAsync), "Response is not as expected!");
        }

        private async Task GetProductsDataAsync(uint startId = uint.MinValue, uint endId = uint.MaxValue)
        {
            List<uint> errorList = new();
            for (uint id = startId; id <= endId; id++)
            {
                if (ProductIds.Contains(id))
                {
                    try
                    {
                        Products.Add(id, await GetProductAsync(id));
                    }
                    catch (Exception)
                    {
                        errorList.Add(id);
                    }
                }
            }
        }

        private async Task CreateProductAsync(DisplayProduct? product)
        {

            using HttpResponseMessage response = await ApiConnection.Client.PostAsJsonAsync("api/Products/add", product);
            if (response.IsSuccessStatusCode)
            {
                Response? r = await response.Content.ReadFromJsonAsync<Response>();
                product = r.Data as DisplayProduct;
                Products.Add(product.Uuid, product);
            }
            else
            {

            }
        }

        private async Task UpdateProductAsync(uint uuid, DisplayProduct product)
        {
            using HttpResponseMessage response = await ApiConnection.Client.PutAsJsonAsync($"api/Products/{uuid}", product);
            if (response.IsSuccessStatusCode)
            {
                if (Products.ContainsKey(product.Uuid))
                {
                    Products.Update(product.Uuid, (DisplayProduct?)(await response.Content.ReadFromJsonAsync<Response>()).Data);
                }
                else
                {
                    Products.Add(product.Uuid, (DisplayProduct?)(await response.Content.ReadFromJsonAsync<Response>()).Data);
                }
            }
            else
            {

            }
        }

        private async Task DeleteProductAsync(uint id)
        {
            using HttpResponseMessage response = await ApiConnection.Client.DeleteAsync($"api/Products/{id}");
            if (response.IsSuccessStatusCode)
            {
                if (Products.ContainsKey(id))
                {
                    _ = Products.Remove(id);
                }
            }
            else
            {

            }
        }

        private async Task<Dictionary<Guid, object>> LoadPricesOfProduct(uint productId)
        {
            Dictionary<Guid, object>? result = new();
            if (Products.TryGetValue(productId, out DisplayProduct? product) && product != null)
            {
                using (priceLoadCts = new())
                {
                    foreach (Guid price in product.Prices)
                    {
                        priceLoadCts.Token.ThrowIfCancellationRequested();
                        try
                        {
                            using HttpResponseMessage response = await ApiConnection.Client.GetAsync($"api/Prices/{price}", priceLoadCts.Token);
                            Response? r = await response.Content.ReadFromJsonAsync<Response>(cancellationToken: priceLoadCts.Token);
                            if (response.IsSuccessStatusCode && r.Data is Price p)
                            {
                                result.Add(price, p);
                            }
                            else
                            {
                                throw new ArgumentNullException(nameof(LoadPricesOfProduct), "Response is not as expected!");
                            }
                        }
                        catch (OperationCanceledException ex)
                        {
                            result.Add(price, ex);
                            break;
                        }
                        catch (Exception ex)
                        {
                            result.Add(price, ex);
                        }
                    }
                }
            }

            return result;
        }

        private async Task<Guid> AddPriceAsync(Price price, CancellationToken token)
        {
            try
            {
                using HttpResponseMessage response = await ApiConnection.Client.PutAsJsonAsync($"api/Prices/add", price, token);
                Response? r = await response.Content.ReadFromJsonAsync<Response>(cancellationToken: token);
                if (response.IsSuccessStatusCode && r.Data is Price p)
                {
                    return p.Uuid;
                }
                else
                {
                    throw new ArgumentNullException(nameof(AddPriceAsync), "Response is not as expected!");
                }
            }
            catch (OperationCanceledException ex)
            {

            }
            catch (Exception ex)
            {

            }
            return Guid.Empty;
        }

        private async Task<Price> UpdatePriceAsync(Price price, CancellationToken token)
        {
            try
            {
                using HttpResponseMessage response = await ApiConnection.Client.PutAsJsonAsync($"api/Prices/{price.Uuid}", price, token);
                Response? r = await response.Content.ReadFromJsonAsync<Response>(cancellationToken: token);
                if (response.IsSuccessStatusCode && r.Data is Price p)
                {
                    return p;
                }
                else
                {
                    throw new ArgumentNullException(nameof(UpdatePriceAsync), "Response is not as expected!");
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

        private async Task<bool> DeletePriceAsync(Guid id, CancellationToken token)
        {
            try
            {
                using HttpResponseMessage response = await ApiConnection.Client.PutAsJsonAsync($"api/Prices/{id}", token);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    throw new ArgumentNullException(nameof(DeletePriceAsync), "Response is not as expected!");
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