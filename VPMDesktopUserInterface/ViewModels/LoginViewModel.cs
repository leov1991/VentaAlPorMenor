using Caliburn.Micro;
using System;
using System.Threading.Tasks;
using VPMDesktopUI.EventModels;
using VPMDesktopUI.Library.API;

namespace VPMDesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
        private string _username = "leovilla91@gmail.com";
        private string _password = "Pass.123";
        private string _errorMessage;
        private readonly IAPIHelper _apiHelper;
        private readonly IEventAggregator _eventAggregator;

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                NotifyOfPropertyChange(() => Username);
                NotifyOfPropertyChange(() => CanLogin);
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogin);

            }
        }

        public bool CanLogin => !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);

        public bool IsErrorVisible => !string.IsNullOrEmpty(ErrorMessage);

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                NotifyOfPropertyChange(() => ErrorMessage);
                NotifyOfPropertyChange(() => IsErrorVisible);
            }
        }

        public LoginViewModel(IAPIHelper apiHelper, IEventAggregator eventAggregator)
        {
            _apiHelper = apiHelper;
            _eventAggregator = eventAggregator;
        }

        public async Task Login()
        {
            try
            {
                ErrorMessage = string.Empty;
                var result = await _apiHelper.Authenticate(Username, Password);
                await _apiHelper.GetLoggedInUserInfo(result.Access_Token);

                _eventAggregator.PublishOnUIThread(new LogOnEvent());

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

        }

    }
}
