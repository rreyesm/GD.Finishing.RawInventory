using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Stock_Finishing.Helpers;
using Stock_Finishing.Models;
using Stock_Finishing.Services;

namespace Stock_Finishing.ViewModels
{
    public partial class ScanRollProductionViewModel : ObservableObject
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
        int ruloID;

        private RuloModel _rulo = new();
        public RuloModel Rulo
        {
            get => _rulo;
            set
            {
                _rulo = value;
                OnPropertyChanged();
            }
        }

        private bool _isRuloAvailable;
        public bool IsRuloAvailable
        {
            get => _isRuloAvailable;
            set
            {
                _isRuloAvailable = value;
                OnPropertyChanged();
            }
        }

        public ScanRollProductionViewModel()
        {
            finishingService = App.Current.Services.GetService<IFinishingService>();
            messageService = App.Current.Services.GetService<IMessageService>();
        }

        [RelayCommand]
        public async Task SearchRulo(object obj)
        {
            try
            {
                IsRuloAvailable = false;

                using (WaitCursorChange.BeginWaitCursorBlock(this))
                {
                    var result = await finishingService.GetRulo(RuloID);

                    if (!result.IsSuccess)
                    {
                        await messageService.ShowAlertAsync(result.Message);
                        return;
                    }

                    if (result.Data == null)
                    {
                        await messageService.ShowAlertAsync("No se encontró el ID de Rulo en la base de datos");
                        return;
                    }

                    var reprocess = await finishingService.GetReprocess(RuloID);

                    if (reprocess.IsSuccess && reprocess.Data.OriginRuloID == RuloID)
                    {
                        await messageService.ShowAlertAsync("El ID ya cuenta con un registro de Rollo de Reproceso");
                        return;
                    }

                    Rulo = result.Data;
                    IsRuloAvailable = Rulo != null;
                }
            }
            catch (Exception ex)
            {
                await messageService.ShowAlertAsync($"Error: {ex.Message}");
            }
        }

        [RelayCommand]
        public async Task SubtractMetersToTheStyle()
        {
            try
            {
                using (WaitCursorChange.BeginWaitCursorBlock(this))
                {
                    SubtractMetersModel subtractMetersModel = new()
                    {
                        ID = RuloID,
                        User = App.UserModel.UserID
                    };

                    var result = await finishingService.SubtractMetersInProductionStyle(subtractMetersModel);

                    if (!result.IsSuccess)
                    {
                        await messageService.ShowErrorAsync(result.Message);
                        return;
                    }

                    await messageService.ShowAlertAsync($"Registro correctamente escaneado, nuevos metros en el estilo: {result.Data}");

                    RuloID = 0;
                }
            }
            catch (Exception ex)
            {
                await messageService.ShowAlertAsync($"Error en escanear rollo: {ex.Message}");
            }
        }
    }
}
