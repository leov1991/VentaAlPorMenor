using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Web.Http;
using VPMDataManager.Library.DataAccess;
using VPMDataManager.Library.Models;

namespace VPMDataManager.Controllers
{

    [Authorize]
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {

        public List<UserModel> GetById()
        {
            string id = RequestContext.Principal.Identity.GetUserId();
            UserData data = new UserData();

            return data.GetUserById(id);

        }

    }
}
