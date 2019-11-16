using Caliburn.Micro;
using VPMDesktopUI.EventModels;

namespace VPMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly SalesViewModel _salesVm;


        public ShellViewModel(IEventAggregator eventAggregator, SalesViewModel salesVm)
        {
            _salesVm = salesVm;
            _eventAggregator = eventAggregator;

            // Para poder usar Handle
            _eventAggregator.Subscribe(this);

            // Iniciar pantalla de Login
            ActivateItem(IoC.Get<LoginViewModel>());
        }

        /// <summary>
        /// Cuando el usuario está logueado, cierra la pantalla de Login
        /// y abre la de ventas
        /// </summary>
        /// <param name="message"></param>
        public void Handle(LogOnEvent message)
        {
            ActivateItem(_salesVm);
        }
    }
}
