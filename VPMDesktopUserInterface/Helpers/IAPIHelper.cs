using System.Threading.Tasks;
using VPMDesktopUI.Models;

namespace VPMDesktopUI.Helpers
{
    public interface IAPIHelper
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);
    }
}