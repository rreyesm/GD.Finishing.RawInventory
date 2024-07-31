using CommunityToolkit.Mvvm.Input;
using DevExpress.Maui.Editors;
using Stock_Finishing.Models;
using Stock_Finishing.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Finishing.ViewModels
{
    public partial class StockProductionViewModel : INotifyPropertyChanged
    {
        private readonly IFinishingService finishingService;
        private readonly IMessageService messageService;

        private decimal _metersScanned;
        private decimal _metersWithoutScanned;

        public ObservableCollection<RollData> RollosData { get; set; }

        private List<OriginModel> _allOrigins;

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal MetersScanned
        {
            get => _metersScanned;
            set
            {
                if (_metersScanned != value)
                {
                    _metersScanned = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal MetersWithoutScanned
        {
            get => _metersWithoutScanned;
            set
            {
                if (_metersWithoutScanned != value)
                {
                    _metersWithoutScanned = value;
                    OnPropertyChanged();
                }
            }
        }

        private OriginModel _selectedOrigin;
        public OriginModel SelectedOrigin
        {
            get => _selectedOrigin;
            set
            {
                if (_selectedOrigin != value)
                {
                    _selectedOrigin = value;
                    OnPropertyChanged();
                    OnOriginSelected();
                }
            }
        }

        public StockProductionViewModel()
        {
            finishingService = App.Current.Services.GetService<IFinishingService>();
            messageService = App.Current.Services.GetService<IMessageService>();
            InitializeStyles();
        }

        public async Task LoadDataAsync()
        {
            var data = await finishingService.GetInventoryMeters(2);
            if (data != null)
            {
                MetersScanned = data.Data.MetersScanned;
                MetersWithoutScanned = data.Data.MetersWithoutScanned;
                InitializeChartData();
            }
        }

        private void InitializeChartData()
        {
            RollosData = new ObservableCollection<RollData>
            {
                new RollData { Category = "Metros escaneados", Quantity = MetersScanned },
                new RollData { Category = "Metros sin escanear", Quantity = MetersWithoutScanned }
            };

            OnPropertyChanged(nameof(RollosData));
        }

        private async Task InitializeStyles()
        {
            var data = await finishingService.GetListOriginData(1);

            if (data != null)
            {
                _allOrigins = data.Data;
            }
            else
            {
                _allOrigins = [];
            }
        }

        private AsyncItemsSourceProvider _autoCompleteItemsSourceProvider;
        public AsyncItemsSourceProvider AutoCompleteItemsSourceProvider
        {
            get
            {
                if (_autoCompleteItemsSourceProvider == null)
                {
                    _autoCompleteItemsSourceProvider = new AsyncItemsSourceProvider();
                    _autoCompleteItemsSourceProvider.ItemsRequested += OnAsyncItemsSourceProviderItemsRequested;
                }
                return _autoCompleteItemsSourceProvider;
            }
        }

        private void OnAsyncItemsSourceProviderItemsRequested(object sender, ItemsRequestEventArgs e)
        {
            e.Request = () =>
            {
                var filteredStyles = _allOrigins
                    .Where(p => p.OriginCode.Contains(e.Text, StringComparison.OrdinalIgnoreCase) ||
                                p.OriginDescription.Contains(e.Text, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                return filteredStyles.Cast<object>();
            };
        }

        [RelayCommand]
        public async Task OnOriginSelected()
        {
            if (SelectedOrigin != null)
            {
                await LoadDataForSelectedStyle(SelectedOrigin.OriginCode);
            }
        }

        private async Task LoadDataForSelectedStyle(string origin)
        {
            try
            {
                IsBusy = true;

                var data = await finishingService.GetInventoryMetersByParam(2, origin);
                if (data != null)
                {
                    MetersScanned = data.Data.MetersScanned;
                    MetersWithoutScanned = data.Data.MetersWithoutScanned;
                    InitializeChartData();
                }
            }
            catch (Exception ex)
            {
                await messageService.ShowErrorAsync($"Error al cargar datos para el estilo {origin}: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
