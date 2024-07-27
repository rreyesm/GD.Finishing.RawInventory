using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Stock_Finishing.Helpers;
using Stock_Finishing.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Finishing.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IUserService _userService;
        private readonly IMessageService messageService;

        [ObservableProperty]
        bool isBusy;

        private string _userName;
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public LoginViewModel()
        {
            _userService = App.Current.Services.GetService<IUserService>();
            messageService = App.Current.Services.GetService<IMessageService>();
        }

        [RelayCommand]
        public async Task Login()
        {
            using (WaitCursorChange.BeginWaitCursorBlock(this))
            {
                try
                {
                    var userToken = await _userService.Authenticate(UserName, Password);
                    if (!userToken.IsSuccess)
                    {
                        await messageService.ShowErrorAsync(userToken.Message);
                        return;
                    }

                    App.UserModel = userToken.Data;

                    App.Current.MainPage = new AppShell();

                }
                catch (Exception ex)
                {
                    await messageService.ShowErrorAsync(ex.Message);
                }
            }
        }
    }
}
