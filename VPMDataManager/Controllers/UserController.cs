using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using VPMDataManager.Library.DataAccess;
using VPMDataManager.Library.Models;
using VPMDataManager.Models;

namespace VPMDataManager.Controllers
{

    [Authorize]
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        [HttpGet]
        public UserModel GetById()
        {
            string id = RequestContext.Principal.Identity.GetUserId();
            UserData data = new UserData();

            return data.GetUserById(id).First();

        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("admin/all")]
        public List<ApplicationUserModel> GetAllUsers()
        {
            List<ApplicationUserModel> output = new List<ApplicationUserModel>();
            using (var context = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                var users = userManager.Users.ToList();
                var roles = context.Roles.ToList();

                foreach (var user in users)
                {
                    ApplicationUserModel u = new ApplicationUserModel
                    {
                        Id = user.Id,
                        Email = user.Email
                    };

                    foreach (var r in user.Roles)
                    {
                        u.Roles.Add(r.RoleId, roles.FirstOrDefault(x => x.Id == r.RoleId).Name);
                    }


                    output.Add(u);
                }

            }

            return output;

        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("admin/roles")]

        public Dictionary<string, string> GetAllRoles()
        {

            using (var context = new ApplicationDbContext())
            {

                var roles = context.Roles.ToDictionary(x => x.Id, x => x.Name);

                return roles;


            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("admin/addToRole")]
        public void AddToRole(UserRolePairModel pair)
        {
            using (var context = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                userManager.AddToRole(pair.UserId, pair.RoleName);

            }

        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("admin/removeFromRole")]
        public void RemoveToRole(UserRolePairModel pair)
        {
            using (var context = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                userManager.RemoveFromRole(pair.UserId, pair.RoleName);

            }
        }
    }
}
