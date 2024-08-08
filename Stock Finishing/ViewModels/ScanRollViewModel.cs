using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Stock_Finishing.Helpers;
using Stock_Finishing.Models;
using Stock_Finishing.Services;

namespace Stock_Finishing.ViewModels
{
    public partial class ScanRollViewModel : ObservableObject
    {
        private readonly IFinishingService finishingService;
        private readonly IMessageService messageService;

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        [ObservableProperty]
        int registerID;

        private FabricInformationDTO _register = new();
        public FabricInformationDTO Register
        {
            get => _register;
            set
            {
                _register = value;
                OnPropertyChanged();
            }
        }

        private bool _isRegisterAvailable;
        public bool IsRegisterAvailable
        {
            get => _isRegisterAvailable;
            set
            {
                _isRegisterAvailable = value;
                OnPropertyChanged();
            }
        }

        public ScanRollViewModel()
        {
            finishingService = App.Current.Services.GetService<IFinishingService>();
            messageService = App.Current.Services.GetService<IMessageService>();
        }

        [RelayCommand]
        public async Task SearchRegister(object obj)
        {
            try
            {
                IsRegisterAvailable = false;

                using (WaitCursorChange.BeginWaitCursorBlock(this))
                {
                    var result = await finishingService.GetFabricInformationForStock(RegisterID);

                    if (!result.IsSuccess)
                    {
                        await messageService.ShowAlertAsync(result.Message);
                        return;
                    }

                    if (result.Data == null)
                    {
                        await messageService.ShowAlertAsync("No se encontró el ID en la base de datos");
                        return;
                    }

                    Register = result.Data;
                    IsRegisterAvailable = Register != null;
                }
            }
            catch (Exception ex)
            {
                await messageService.ShowAlertAsync($"Error: {ex.Message}");
            }
        }

        [RelayCommand]
        public async Task SubtractMeters()
        {
            try
            {
                using (WaitCursorChange.BeginWaitCursorBlock(this))
                {
                    SubtractMetersModel subtractMetersModel = new()
                    {
                        ID = RegisterID,
                        User = App.UserModel.UserID
                    };

                    var result = await finishingService.SubtractMetersByType(Register.Type.ToLower(), subtractMetersModel);

                    if (!result.IsSuccess)
                    {
                        await messageService.ShowErrorAsync(result.Message);
                        return;
                    }

                    await messageService.ShowAlertAsync($"Registro correctamente escaneado, nuevos metros en el estilo: {result.Data}");

                    RegisterID = 0;
                    Register = new FabricInformationDTO();
                }
            }
            catch (Exception ex)
            {
                await messageService.ShowAlertAsync($"Error en escanear rollo: {ex.Message}");
            }
        }

    }
}
