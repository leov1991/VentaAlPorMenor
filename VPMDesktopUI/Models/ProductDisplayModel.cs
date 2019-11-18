namespace VPMDesktopUI.Models
{
    public class ProductDisplayModel : ModelBase
    {

        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal RetailPrice { get; set; }
        private int _quantityInStock;

        public int QuantityInStock
        {
            get { return _quantityInStock; }
            set
            {
                _quantityInStock = value;
                OnPropertyChanged(nameof(QuantityInStock));
            }
        }

        public bool IsTaxable { get; set; }
    }
}