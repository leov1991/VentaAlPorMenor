using System;
using System.Configuration;
using System.Globalization;

namespace VPMDataManager.Library
{
    public class ConfigHelper
    {
        // TODO: Mover a la API
        public static decimal GetTaxRate()
        {
            string rateText = ConfigurationManager.AppSettings["taxRate"];

            bool isValidTaxRate = Decimal.TryParse(rateText, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal output);

            if (isValidTaxRate == false)
                throw new ConfigurationErrorsException("El porcentaje de impuesto no está definido correctamente");

            return output;
        }
    }
}
