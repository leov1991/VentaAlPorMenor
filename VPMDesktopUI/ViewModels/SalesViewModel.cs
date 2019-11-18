using AutoMapper;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using VPMDesktopUI.Library.API;
using VPMDesktopUI.Library.Helpers;
using VPMDesktopUI.Library.Models;
using VPMDesktopUI.Models;

namespace VPMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private BindingList<ProductDisplayModel> _products;
        private BindingList<CartItemDisplayModel> _cart = new BindingList<CartItemDisplayModel>();
        private int _itemQuantity = 1;
        private readonly IProductEndpoint _productEndpoint;
        private readonly IConfigHelper _configHelper;
        private readonly ISaleEndpoint _saleEndpoint;
        private readonly IMapper _mapper;
        private readonly IWindowManager _window;

        public BindingList<ProductDisplayModel> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        private ProductDisplayModel _selectedProduct;

        public ProductDisplayModel SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        public BindingList<CartItemDisplayModel> Cart
        {
            get { return _cart; }
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }

        private CartItemDisplayModel _selectedCartProduct;

        public CartItemDisplayModel SelectedCartProduct
        {
            get { return _selectedCartProduct; }
            set
            {
                _selectedCartProduct = value;

                NotifyOfPropertyChange(() => SelectedCartProduct);
                NotifyOfPropertyChange(() => CanRemoveFromCart);
            }
        }


        public int ItemQuantity
        {
            get { return _itemQuantity; }
            set
            {
                _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
                NotifyOfPropertyChange(() => CanAddToCart);

            }
        }

        public string Subtotal => CalculateSubtotal().ToString("C");

        public string Tax => CalculateTax().ToString("C");

        public string Total => (CalculateSubtotal() + CalculateTax()).ToString("C");

        public bool CanAddToCart => ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity;
        public bool CanRemoveFromCart => SelectedCartProduct != null && SelectedCartProduct.QuantityInCart > 0;

        public bool CanCheckout => Cart.Count > 0;

        public SalesViewModel(IProductEndpoint productEndpoint, IConfigHelper configHelper, ISaleEndpoint saleEndpoint,
            IMapper mapper, IWindowManager window)
        {
            _productEndpoint = productEndpoint;
            _configHelper = configHelper;
            _saleEndpoint = saleEndpoint;
            _mapper = mapper;
            _window = window;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            try
            {
                await LoadProducts();
            }
            catch (Exception ex)
            {
                dynamic settings = new ExpandoObject();
                settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                settings.ResizeMode = ResizeMode.NoResize;
                settings.Title = "Error";
                var msg = IoC.Get<StatusInfoViewModel>();
                if (ex.Message == "Unauthorized")
                {
                    msg.UpdateMessage("Acceso no autorizado", "No tiene permisos para interactuar con esta vista.");
                }
                else
                {
                    msg.UpdateMessage("Lista de productos", "Ocurrió un error recuperando la lista de productos.");
                }

                await _window.ShowDialogAsync(msg, null, settings);

                await TryCloseAsync();
            }
        }

        private async Task LoadProducts()
        {
            var productList = await _productEndpoint.GetAll();
            var products = _mapper.Map<List<ProductDisplayModel>>(productList);
            Products = new BindingList<ProductDisplayModel>(products);
        }

        private decimal CalculateTax()
        {
            decimal taxAmount = 0;
            decimal taxRate = _configHelper.GetTaxRate() / 100;

            taxAmount = Cart.Where(i => i.Product.IsTaxable)
                .Sum(i => i.Product.RetailPrice * i.QuantityInCart * taxRate);


            return taxAmount;
        }

        private decimal CalculateSubtotal()
        {
            decimal subtotal = 0;

            subtotal = Cart.Sum(i => i.Product.RetailPrice * i.QuantityInCart);

            return subtotal;
        }

        public async Task Checkout()
        {
            SaleModel sale = new SaleModel();
            foreach (var item in Cart)
            {
                sale.SaleDetails.Add(new SaleDetailModel
                {
                    ProductId = item.Product.Id,
                    Quantity = item.QuantityInCart
                });
            }

            await _saleEndpoint.PostSale(sale);

            await ResetViewModel();



        }

        public void AddToCart()
        {
            // Si el producto existe en el carrito, actualizar cantidad, si no, crear uno nuevo.
            CartItemDisplayModel existingItem = Cart.FirstOrDefault(i => i.Product == SelectedProduct);

            if (existingItem != null)
            {
                existingItem.QuantityInCart += ItemQuantity;

            }
            else
            {
                CartItemDisplayModel item = new CartItemDisplayModel
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity
                };

                Cart.Add(item);

            }

            SelectedProduct.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1;

            NotifyChangesInCart();


        }

        public void RemoveFromCart()
        {
            SelectedCartProduct.Product.QuantityInStock += 1;

            if (SelectedCartProduct.QuantityInCart > 1)
            {
                SelectedCartProduct.QuantityInCart -= 1;
            }
            else
            {
                Cart.Remove(SelectedCartProduct);
            }

            NotifyOfPropertyChange(() => CanAddToCart);

            NotifyChangesInCart();
        }


        private async Task ResetViewModel()
        {
            Cart = new BindingList<CartItemDisplayModel>();
            await LoadProducts();
            NotifyChangesInCart();
        }
        private void NotifyChangesInCart()
        {
            NotifyOfPropertyChange(() => Subtotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckout);
        }
    }
}
