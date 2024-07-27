using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Stock_Finishing.Helpers;
using Stock_Finishing.Models;
using Stock_Finishing.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        int ruloMigrationID;

        private RuloMigrationModel _ruloMigration = new();
        public RuloMigrationModel RuloMigration
        {
            get => _ruloMigration;
            set
            {
                _ruloMigration = value;
                OnPropertyChanged();
            }
        }

        private bool _isRuloMigrationAvailable;
        public bool IsRuloMigrationAvailable
        {
            get => _isRuloMigrationAvailable;
            set
            {
                _isRuloMigrationAvailable = value;
                OnPropertyChanged();
            }
        }

        public ScanRollViewModel()
        {
            finishingService = App.Current.Services.GetService<IFinishingService>();
            messageService = App.Current.Services.GetService<IMessageService>();
        }

        [RelayCommand]
        public async Task SearchRuloMigration(object obj)
        {
            try
            {
                IsRuloMigrationAvailable = false;

                using (WaitCursorChange.BeginWaitCursorBlock(this))
                {
                    var result = await finishingService.GetRuloMigration(RuloMigrationID);

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

                    RuloMigration = result.Data;
                    IsRuloMigrationAvailable = RuloMigration != null;
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
                        RuloMigrationID = RuloMigrationID,
                        User = App.UserModel.UserID
                    };

                    var result = await finishingService.SubtractMetersToTheStyle(subtractMetersModel);

                    if (!result.IsSuccess)
                    {
                        await messageService.ShowErrorAsync(result.Message);
                        return;
                    }

                    await messageService.ShowAlertAsync($"Registro correctamente escaneado, nuevos metros en el estilo: {result.Data}");

                    RuloMigrationID = 0;
                }
            }
            catch (Exception ex)
            {
                await messageService.ShowAlertAsync($"Error en escanear rollo: {ex.Message}");
            }
        }

    }
}
