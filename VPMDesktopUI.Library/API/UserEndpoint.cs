using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using VPMDesktopUI.Library.Models;

namespace VPMDesktopUI.Library.API
{
    public class UserEndpoint : IUserEndpoint
    {
        private readonly IAPIHelper _apiHelper;

        public UserEndpoint(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<List<UserModel>> GetAllUsers()
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("api/user/admin/all"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<UserModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }

            }
        }

    }
}
