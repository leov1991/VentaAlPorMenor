using System;
using System.Configuration;
using System.Globalization;

namespace VPMDesktopUI.Library.Helpers
{
    public class ConfigHelper : IConfigHelper
    {
        public decimal GetTaxRate()
        {
            string rateText = ConfigurationManager.AppSettings["taxRate"];

            bool isValidTaxRate = Decimal.TryParse(rateText, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal output);

            if (isValidTaxRate == false)
                throw new ConfigurationErrorsException("El porcentaje de impuesto no está definido correctamente");

            return output;
        }
    }
}
