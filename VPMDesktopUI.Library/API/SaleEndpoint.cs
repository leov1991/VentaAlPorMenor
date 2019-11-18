using System;
using System.Net.Http;
using System.Threading.Tasks;
using VPMDesktopUI.Library.Models;

namespace VPMDesktopUI.Library.API
{
    public class SaleEndpoint : ISaleEndpoint
    {
        private const string baseEndpoint = "/api/sale";
        private readonly IAPIHelper _apiHelper;

        public SaleEndpoint(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task PostSale(SaleModel sale)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync(baseEndpoint, sale))
            {
                if (response.IsSuccessStatusCode)
                {
                    // TODO: Log succesfull call??
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }


    }
}
