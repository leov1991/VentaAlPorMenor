﻿using Caliburn.Micro;
using VPMDesktopUI.EventModels;
using VPMDesktopUI.Library.API;
using VPMDesktopUI.Library.Models;

namespace VPMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly SalesViewModel _salesVm;
        private readonly ILoggedInUserModel _user;
        private readonly IAPIHelper _apiHelper;
        private bool _isAccountVisible;

        public bool IsLoggedIn => !string.IsNullOrEmpty(_user.Token);

        public ShellViewModel(IEventAggregator eventAggregator, SalesViewModel salesVm, ILoggedInUserModel user,
            IAPIHelper apiHelper)
        {
            _salesVm = salesVm;
            _user = user;
            _apiHelper = apiHelper;
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
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public void ExitApplication()
        {
            TryClose();
        }

        public void LogOut()
        {

            _user.ResetUser();
            _apiHelper.LogOffUser();
            ActivateItem(IoC.Get<LoginViewModel>());
            NotifyOfPropertyChange(() => IsLoggedIn);
        }
    }
}
