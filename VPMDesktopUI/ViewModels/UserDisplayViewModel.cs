using Caliburn.Micro;
using System;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
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

        private UserModel _selectedUser;

        public UserModel SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                SelectedUserName = value.Email;

                SelectedUserRoles.Clear();
                SelectedUserRoles = new BindingList<string>(value.Roles.Select(x => x.Value).ToList());
                LoadRoles();

                NotifyOfPropertyChange(() => SelectedUser);
            }
        }

        private string _selectedUserName;

        public string SelectedUserName
        {
            get { return _selectedUserName; }
            set
            {
                _selectedUserName = value;
                NotifyOfPropertyChange(() => SelectedUserName);
            }
        }

        private BindingList<string> _selectedUserRoles = new BindingList<string>();

        public BindingList<string> SelectedUserRoles
        {
            get { return _selectedUserRoles; }
            set
            {
                _selectedUserRoles = value;
                NotifyOfPropertyChange(() => SelectedUserRoles);
            }
        }

        private BindingList<string> _availableRoles = new BindingList<string>();

        public BindingList<string> AvailableRoles
        {
            get { return _availableRoles; }
            set
            {
                _availableRoles = value;
                NotifyOfPropertyChange(() => AvailableRoles);
            }
        }

        private string _selectedRoleToRemove;

        public string SelectedRoleToRemove
        {
            get { return _selectedRoleToRemove; }
            set
            {
                _selectedRoleToRemove = value;
                NotifyOfPropertyChange(() => SelectedRoleToRemove);
            }
        }

        private string _selectedRoleToAdd;

        public string SelectedRoleToAdd
        {
            get { return _selectedRoleToAdd; }
            set
            {
                _selectedRoleToAdd = value;
                NotifyOfPropertyChange(() => SelectedRoleToAdd);
            }
        }


        //public bool CanRemoveFromRole => true;
        //public bool CanAddToRole => true;
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

                await _window.ShowDialogAsync(msg, null, settings);

                await TryCloseAsync();
            }
        }

        private async Task LoadUsers()
        {
            var userList = await _userEndpoint.GetAllUsers();
            Users = new BindingList<UserModel>(userList);

        }

        private async Task LoadRoles()
        {
            AvailableRoles.Clear();
            var roleList = await _userEndpoint.GetAllRoles();

            foreach (var role in roleList)
            {
                if (SelectedUserRoles.IndexOf(role.Value) < 0)
                {
                    AvailableRoles.Add(role.Value);
                }

            }


        }

        public async void AddToRole()
        {
            await _userEndpoint.AddUserToRole(SelectedUser.Id, SelectedRoleToAdd);
            SelectedUserRoles.Add(SelectedRoleToAdd);
            AvailableRoles.Remove(SelectedRoleToAdd);
        }

        public async void RemoveFromRole()
        {
            await _userEndpoint.RemoveUserFromRole(SelectedUser.Id, SelectedRoleToRemove);
            AvailableRoles.Add(SelectedRoleToRemove);
            SelectedUserRoles.Remove(SelectedRoleToRemove);
        }
    }
}
