namespace VPMDesktopUI.Models
{
    public class CartItemDisplayModel : ModelBase
    {
        public ProductDisplayModel Product { get; set; }
        private int _quantityInCart;



        public int QuantityInCart
        {
            get { return _quantityInCart; }
            set
            {
                _quantityInCart = value;
                OnPropertyChanged(nameof(QuantityInCart));
                OnPropertyChanged(nameof(DisplayText));
            }
        }


        public string DisplayText => $"{Product.ProductName}  ({QuantityInCart})";

    }
}
