using System.Threading.Tasks;
using VPMDesktopUI.Library.Models;

namespace VPMDesktopUI.Library.API
{
    public interface IAPIHelper
    {
        System.Net.Http.HttpClient ApiClient { get; }

        Task<AuthenticatedUser> Authenticate(string username, string password);
        Task GetLoggedInUserInfo(string token);
        void LogOffUser();
    }
}