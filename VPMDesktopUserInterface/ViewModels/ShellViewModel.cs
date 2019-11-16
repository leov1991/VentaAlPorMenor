using Caliburn.Micro;

namespace VPMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {
        private LoginViewModel _loginVm { get; }
        public ShellViewModel(LoginViewModel loginvm)
        {
            _loginVm = loginvm;
            ActivateItem(_loginVm);
        }
    }
}
