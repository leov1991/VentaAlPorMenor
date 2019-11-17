using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Threading.Tasks;
using System.Windows;
using VPMDesktopUI.Library.API;
using VPMDesktopUI.Library.Models;

namespace VPMDesktopUI.ViewModels
{
    public class UserDisplayViewModel : Screen
    {
        private readonly IUserEndpoint _userEndpoint;
        private readonly IWindowManager _window;

        private BindingList<UserModel> _users;

        public BindingList<UserModel> Users
        {

            get => _users;
            set
            {
                _users = value;
                NotifyOfPropertyChange(() => Users);
            }
        }


        public UserDisplayViewModel(IUserEndpoint userEndpoint, IWindowManager window)
        {
            _userEndpoint = userEndpoint;
            _window = window;
        }
        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            try
            {
                await LoadUsers();
            }
            catch (Exception ex)
            {
                dynamic settings = new ExpandoObject();
                settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                settings.ResizeMode = ResizeMode.NoResize;
                settings.Title = "Error";
                var msg = IoC.Get<StatusInfoViewModel>();
                if (ex.Message == "Unauthorized")
                {
                    msg.UpdateMessage("Acceso no autorizado", "No tiene permisos para interactuar con esta vista.");
                }
                else
                {
                    msg.UpdateMessage("Lista de productos", "Ocurrió un error recuperando la lista de productos.");
                }

                _window.ShowDialog(msg, null, settings);

                TryClose();
            }
        }

        private async Task LoadUsers()
        {
            var userList = await _userEndpoint.GetAllUsers();
            Users = new BindingList<UserModel>(userList);

        }

    }
}
