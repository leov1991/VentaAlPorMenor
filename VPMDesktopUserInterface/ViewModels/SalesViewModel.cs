using Caliburn.Micro;
using System.ComponentModel;

namespace VPMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private BindingList<string> _products;

        public BindingList<string> Products
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

        public SalesViewModel()
        {

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
