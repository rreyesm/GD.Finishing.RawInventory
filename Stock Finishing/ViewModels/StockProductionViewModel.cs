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

        private List<StyleModel> _allStyles;

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

        private StyleModel _selectedStyle;
        public StyleModel SelectedStyle
        {
            get => _selectedStyle;
            set
            {
                if (_selectedStyle != value)
                {
                    _selectedStyle = value;
                    OnPropertyChanged();
                    OnStyleSelected();
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
            var data = await finishingService.GetListStyleData(2);

            if (data != null)
            {
                _allStyles = data.Data;
            }
            else
            {
                _allStyles = [];
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
                var filteredStyles = _allStyles
                    .Where(p => p.StyleName.Contains(e.Text, StringComparison.OrdinalIgnoreCase) ||
                                p.StyleCode.Contains(e.Text, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                return filteredStyles.Cast<object>();
            };
        }

        [RelayCommand]
        public async Task OnStyleSelected()
        {
            if (SelectedStyle != null)
            {
                await LoadDataForSelectedStyle(SelectedStyle.StyleCode);
            }
        }

        private async Task LoadDataForSelectedStyle(string styleCode)
        {
            try
            {
                IsBusy = true;

                var data = await finishingService.GetInventoryMetersByStyle(2, styleCode);
                if (data != null)
                {
                    MetersScanned = data.Data.MetersScanned;
                    MetersWithoutScanned = data.Data.MetersWithoutScanned;
                    InitializeChartData();
                }
            }
            catch (Exception ex)
            {
                await messageService.ShowErrorAsync($"Error al cargar datos para el estilo {styleCode}: {ex.Message}");
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
