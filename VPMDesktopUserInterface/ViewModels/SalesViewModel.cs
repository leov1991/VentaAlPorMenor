using Caliburn.Micro;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using VPMDesktopUI.Library.API;
using VPMDesktopUI.Library.Helpers;
using VPMDesktopUI.Library.Models;

namespace VPMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private BindingList<ProductModel> _products;

        public BindingList<ProductModel> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        private ProductModel _selectedProduct;

        public ProductModel SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }


        private BindingList<CartItemModel> _cart = new BindingList<CartItemModel>();

        public BindingList<CartItemModel> Cart
        {
            get { return _cart; }
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }


        private int _itemQuantity = 1;
        private readonly IProductEndpoint _productEndpoint;
        private readonly IConfigHelper _configHelper;

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

        public string Subtotal
        {
            get
            {
                return CalculateSubtotal().ToString("C");
            }
        }



        public string Tax
        {
            get
            {
                return CalculateTax().ToString("C");
            }
        }

        private decimal CalculateTax()
        {
            decimal taxAmount = 0;
            decimal taxRate = _configHelper.GetTaxRate() / 100;

            foreach (var item in Cart)
            {

                if (item.Product.IsTaxable)
                {
                    taxAmount += item.Product.RetailPrice * item.QuantityInCart * taxRate;
                }
            }

            return taxAmount;
        }

        public string Total
        {
            get
            {
                decimal total = (CalculateSubtotal() + CalculateTax());
                return total.ToString("C");
            }
        }

        public bool CanAddToCart => ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity;
        public bool CanRemoveFromCart => false;

        public bool CanCheckout => false;

        public SalesViewModel(IProductEndpoint productEndpoint, IConfigHelper configHelper)
        {
            _productEndpoint = productEndpoint;
            _configHelper = configHelper;
        }

        protected override async void OnViewLoaded(object view)
        {
            await LoadProducts();
        }

        private async Task LoadProducts()
        {
            var products = await _productEndpoint.GetAll();
            Products = new BindingList<ProductModel>(products);
        }


        private decimal CalculateSubtotal()
        {
            decimal subtotal = 0;
            foreach (var item in Cart)
            {

                subtotal += item.Product.RetailPrice * item.QuantityInCart;
            }

            return subtotal;
        }
        public void Checkout()
        {

        }
        public void AddToCart()
        {
            // Si el producto existe en el carrito, actualizar cantidad, si no, crear uno nuevo.
            CartItemModel existingItem = Cart.FirstOrDefault(i => i.Product == SelectedProduct);

            if (existingItem != null)
            {
                existingItem.QuantityInCart += ItemQuantity;
                //// HACK
                Cart.Remove(existingItem);
                Cart.Add(existingItem);

            }
            else
            {
                CartItemModel item = new CartItemModel
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity
                };

                Cart.Add(item);

            }

            SelectedProduct.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1;            
            NotifyOfPropertyChange(() => Subtotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);


        }

        public void RemoveFromCart()
        {

            NotifyOfPropertyChange(() => Subtotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
        }



    }
}
