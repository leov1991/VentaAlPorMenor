using Caliburn.Micro;
using System.ComponentModel;
using System.Threading.Tasks;
using VPMDesktopUI.Library.API;
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

        private BindingList<string> _cart;

        public BindingList<string> Cart
        {
            get { return _cart; }
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }


        private int _itemQuantity;
        private readonly IProductEndpoint _productEndpoint;

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

        public string Subtotal => "$0.00";
        public string Tax => "$0.00";
        public string Total => "$0.00";

        public bool CanAddToCart { get; set; } // Comprobar que hay producto seleccionado y cantidad
        public bool CanRemoveFromCart { get; set; } // Comprobar que hay producto seleccionado en carrito

        public bool CanCheckout { get; set; } // Comprobar que el carrito no está vacío

        public SalesViewModel(IProductEndpoint productEndpoint)
        {
            _productEndpoint = productEndpoint;

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

        public void Checkout()
        {

        }
        public void AddToCart()
        {

        }

        public void RemoveToCart()
        {

        }



    }
}
