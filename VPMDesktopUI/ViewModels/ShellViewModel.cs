using Caliburn.Micro;
using System.Threading;
using System.Threading.Tasks;
using VPMDesktopUI.EventModels;
using VPMDesktopUI.Library.API;
using VPMDesktopUI.Library.Models;

namespace VPMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private readonly IEventAggregator _eventAggregator;        
        private readonly ILoggedInUserModel _user;
        private readonly IAPIHelper _apiHelper;

        public bool IsLoggedIn => !string.IsNullOrEmpty(_user.Token);

        public ShellViewModel(IEventAggregator eventAggregator, ILoggedInUserModel user,
            IAPIHelper apiHelper)
        {            
            _user = user;
            _apiHelper = apiHelper;
            _eventAggregator = eventAggregator;

            // Para poder usar Handle
            _eventAggregator.SubscribeOnPublishedThread(this);

            // Iniciar pantalla de Login
            ActivateItemAsync(IoC.Get<LoginViewModel>(), new CancellationToken());
        }

        public async Task ExitApplication()
        {
            await TryCloseAsync();
        }

        public async Task UserManagment()
        {
            await ActivateItemAsync(IoC.Get<UserDisplayViewModel>(), new CancellationToken());
        }

        public async Task LogOut()
        {

            _user.ResetUser();
            _apiHelper.LogOffUser();
            await ActivateItemAsync(IoC.Get<LoginViewModel>(), new CancellationToken());
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        /// <summary>
        /// Cuando el usuario está logueado, cierra la pantalla de Login
        /// y abre la de ventas
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {

            await ActivateItemAsync(IoC.Get<SalesViewModel>(), cancellationToken);
            NotifyOfPropertyChange(() => IsLoggedIn);
        }
    }
}
