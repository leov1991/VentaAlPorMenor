using AutoMapper;
using Caliburn.Micro;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
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
            IMapper mapper)
        {
            _productEndpoint = productEndpoint;
            _configHelper = configHelper;
            _saleEndpoint = saleEndpoint;
            _mapper = mapper;
        }

        protected override async void OnViewLoaded(object view)
        {
            await LoadProducts();
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

            Cart.Clear();
            NotifyChangesInCart();

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

            if(SelectedCartProduct.QuantityInCart > 1)
            {
                SelectedCartProduct.QuantityInCart -= 1;
            }
            else
            {
                Cart.Remove(SelectedCartProduct);
            }
            
            

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
