using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using WireDev.Erp.V1.Models.Authentication;
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

        private Dictionary<uint, Product>? _products = null;
        public Dictionary<uint, Product> Products
        {
            get => _products;
            set
            {
                if (_products != value)
                {
                    _products = value;
                    base.OnPropertyChanged(nameof(ProductIds));
                }
            }
        }

        private Product? _selectedProduct = null;
        public Product SelectedProduct
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
                { 9999, new Product(9999) { Active = true, Name = "Test_Product" } }
            };
        }

        private bool CanGetProducts = true;
        private ICommand? _getProductsCommand;
        public ICommand GetProductsCommand => _getProductsCommand ??= new RelayCommand(param => GetProducts().ConfigureAwait(true), param => CanGetProducts);
        private readonly bool CanGetProductsData = true;
        private ICommand? _getProductsDataCommand;
        public ICommand GetProductsDataCommand => _getProductsDataCommand ??= new RelayCommand(param => GetProducts().ConfigureAwait(true), param => CanGetProductsData);

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

        private async Task<List<uint>> GetProducts()
        {
            CanGetProducts = false;

            try
            {
                HttpResponseMessage response = await ApiConnection.Client.GetAsync($"api/Products/all");
                Response? r = await response.Content.ReadFromJsonAsync<Response>();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return ProductIds = r.Data is List<uint> list ? list : throw new ArgumentNullException("Respose is not as expected!");
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

        private async Task<Product> GetProduct(uint id)
        {
            HttpResponseMessage response = await ApiConnection.Client.GetAsync($"api/Products/{id}");
            Response? r = await response.Content.ReadFromJsonAsync<Response>();

            return response.IsSuccessStatusCode && (r.Data is Product p) ? p : throw new ArgumentNullException("Respose is not as expected!");
        }

        private async Task GetProductsData(uint startId = uint.MinValue, uint endId = uint.MaxValue)
        {
            for (uint id = startId; id <= endId; id++)
            {
                if (ProductIds.Contains(id))
                {
                    try
                    {
                        Products.Add(id, await GetProduct(id));
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }

        public void ProductsView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView lv)
            {
                SelectedProduct = (Product)lv.SelectedItem;
            }
        }
    }
}