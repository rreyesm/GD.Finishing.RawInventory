using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Stock_Finishing.Helpers;
using Stock_Finishing.Models;
using Stock_Finishing.Services;

namespace Stock_Finishing.ViewModels
{
    public partial class ScanRollReprocessViewModel : ObservableObject
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
        int reprocessID;

        private ReprocessModel _reprocess = new();
        public ReprocessModel Reprocess
        {
            get => _reprocess;
            set
            {
                _reprocess = value;
                OnPropertyChanged();
            }
        }

        private bool _isReprocessAvailable;
        public bool IsReprocessAvailable
        {
            get => _isReprocessAvailable;
            set
            {
                _isReprocessAvailable = value;
                OnPropertyChanged();
            }
        }

        public ScanRollReprocessViewModel()
        {
            finishingService = App.Current.Services.GetService<IFinishingService>();
            messageService = App.Current.Services.GetService<IMessageService>();
        }

        [RelayCommand]
        public async Task SearchReprocess(object obj)
        {
            try
            {
                IsReprocessAvailable = false;

                using (WaitCursorChange.BeginWaitCursorBlock(this))
                {
                    var result = await finishingService.GetReprocess(ReprocessID);

                    if (!result.IsSuccess)
                    {
                        await messageService.ShowAlertAsync(result.Message);
                        return;
                    }

                    if (result.Data == null)
                    {
                        await messageService.ShowAlertAsync("No se encontró el ID de Reproceso en la base de datos");
                        return;
                    }

                    if (result.Data.DestinationRuloID != null)
                    {
                        await messageService.ShowAlertAsync("El ID ya cuenta con un registro de Rollo de Producción");
                        return;
                    }

                    Reprocess = result.Data;
                    IsReprocessAvailable = Reprocess != null;
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
                        ID = ReprocessID,
                        User = App.UserModel.UserID
                    };

                    var result = await finishingService.SubtractMetersInReprocessStyle(subtractMetersModel);

                    if (!result.IsSuccess)
                    {
                        await messageService.ShowErrorAsync(result.Message);
                        return;
                    }

                    await messageService.ShowAlertAsync($"Registro correctamente escaneado, nuevos metros en el estilo: {result.Data}");

                    ReprocessID = 0;
                }
            }
            catch (Exception ex)
            {
                await messageService.ShowAlertAsync($"Error en escanear rollo: {ex.Message}");
            }
        }
    }
}
