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
    public partial class StockViewModel : INotifyPropertyChanged
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

        public StockViewModel()
        {
            finishingService = App.Current.Services.GetService<IFinishingService>();
            messageService = App.Current.Services.GetService<IMessageService>();
            InitializeStyles();
        }

        public async Task LoadDataAsync()
        {
            var data = await finishingService.GetInventoryMeters();
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

        private void InitializeStyles()
        {
            _allStyles = new List<StyleModel>
            {
                new StyleModel { StyleCode = "15V99", StyleName = "33864471" },
                new StyleModel { StyleCode = "15V99", StyleName = "33869474" },
                new StyleModel { StyleCode = "15V99", StyleName = "34405472" },
                new StyleModel { StyleCode = "15V99", StyleName = "34405475" },
                new StyleModel { StyleCode = "15V99", StyleName = "41647080" },
                new StyleModel { StyleCode = "15V99", StyleName = "41647089" },
                new StyleModel { StyleCode = "15V99", StyleName = "41648083" },
                new StyleModel { StyleCode = "15V99", StyleName = "42013009" },
                new StyleModel { StyleCode = "15V99", StyleName = "42022001" },
                new StyleModel { StyleCode = "15V99", StyleName = "42022002" },
                new StyleModel { StyleCode = "15V99", StyleName = "42040012" },
                new StyleModel { StyleCode = "15V99", StyleName = "42065013" },
                new StyleModel { StyleCode = "15V99", StyleName = "42095030" },
                new StyleModel { StyleCode = "15V99", StyleName = "42098031" },
                new StyleModel { StyleCode = "15V99", StyleName = "42098032" },
                new StyleModel { StyleCode = "15V99", StyleName = "42098033" },
                new StyleModel { StyleCode = "15V99", StyleName = "42098034" },
                new StyleModel { StyleCode = "15V99", StyleName = "42148086" },
                new StyleModel { StyleCode = "15V99", StyleName = "42149098" },
                new StyleModel { StyleCode = "15V99", StyleName = "42190146" },
                new StyleModel { StyleCode = "15V99", StyleName = "42191127" },
                new StyleModel { StyleCode = "15V99", StyleName = "42191134" },
                new StyleModel { StyleCode = "15V99", StyleName = "42193128" },
                new StyleModel { StyleCode = "15V99", StyleName = "42193135" },
                new StyleModel { StyleCode = "15V99", StyleName = "45624011" },
                new StyleModel { StyleCode = "15V99", StyleName = "45636014" },
                new StyleModel { StyleCode = "15V99", StyleName = "45667039" },
                new StyleModel { StyleCode = "15V99", StyleName = "45667040" },
                new StyleModel { StyleCode = "15V99", StyleName = "45667041" },
                new StyleModel { StyleCode = "15V99", StyleName = "45667042" },
                new StyleModel { StyleCode = "15V99", StyleName = "45668047" },
                new StyleModel { StyleCode = "15V99", StyleName = "45673064" },
                new StyleModel { StyleCode = "15V99", StyleName = "45674087" },
                new StyleModel { StyleCode = "15V99", StyleName = "45683085" },
                new StyleModel { StyleCode = "16B36:L58", StyleName = "Africa Jungle" },
                new StyleModel { StyleCode = "88Z32:P02", StyleName = "Amalfi Sunset" },
                new StyleModel { StyleCode = "08D07:L55", StyleName = "Angola New Shine" },
                new StyleModel { StyleCode = "12D77:P61", StyleName = "Aquarius R Ultramarine" },
                new StyleModel { StyleCode = "05D77:L67", StyleName = "Aries Ultramarine" },
                new StyleModel { StyleCode = "19D74:L56", StyleName = "Avatar Agean Blue" },
                new StyleModel { StyleCode = "25D62:C50", StyleName = "Baja Cross Black" },
                new StyleModel { StyleCode = "50D43:", StyleName = "Basica 24 Dips" },
                new StyleModel { StyleCode = "50D44:", StyleName = "Basica 24 Dips" },
                new StyleModel { StyleCode = "35D33/2", StyleName = "Basica B/B" },
                new StyleModel { StyleCode = "35D62-", StyleName = "Basica Black" },
                new StyleModel { StyleCode = "35D81-", StyleName = "Basica Black Black" },
                new StyleModel { StyleCode = "11D38:L67O", StyleName = "Bohemia Ocean Blue" },
                new StyleModel { StyleCode = "46D9B:A02", StyleName = "Bravo Ice Blue" },
                new StyleModel { StyleCode = "11I51:L92", StyleName = "Divine PPT" },
                new StyleModel { StyleCode = "92D62:P04", StyleName = "Fantasy Black Black" },
                new StyleModel { StyleCode = "21D62:L81", StyleName = "Feeling´s Black Black" },
                new StyleModel { StyleCode = "10B1A:L02", StyleName = "Freedom River Blue" },
                new StyleModel { StyleCode = "15G99", StyleName = "Guia de acabado" },
                new StyleModel { StyleCode = "08D74:L67", StyleName = "Honey Agean Blue" },
                new StyleModel { StyleCode = "08D33:L81", StyleName = "Honey Black Overdye Black" },
                new StyleModel { StyleCode = "86Z62:P04", StyleName = "Ibiza Black" },
                new StyleModel { StyleCode = "12I2C:P02", StyleName = "Icon Storm Blue" },
                new StyleModel { StyleCode = "18D26-A58", StyleName = "Kenya Blackdigo" },
                new StyleModel { StyleCode = "15D5C:P02", StyleName = "Life Stone Blue" },
                new StyleModel { StyleCode = "12D62:P74", StyleName = "Lucca Black Black" },
                new StyleModel { StyleCode = "28D77:C58", StyleName = "Marvel Ultramarine" },
                new StyleModel { StyleCode = "15D74:L51", StyleName = "Mercury Agean Blue" },
                new StyleModel { StyleCode = "25D74:C11", StyleName = "Osaka Agean Blue" },
                new StyleModel { StyleCode = "13D77:L56", StyleName = "Rocky Ultramarine" },
                new StyleModel { StyleCode = "12D2B:L03", StyleName = "Rodeo 40 Navy Blue" },
                new StyleModel { StyleCode = "12D2B:L02", StyleName = "Rodeo Navy Blue" },
                new StyleModel { StyleCode = "10D74:L75B", StyleName = "Romance Agean Blue" },
                new StyleModel { StyleCode = "14B99:A02", StyleName = "Royal Zafiro" },
                new StyleModel { StyleCode = "15D4C:A02", StyleName = "Rumba Midnight Blue" },
                new StyleModel { StyleCode = "15D5B:A02", StyleName = "Rumba Midnight Blue" },
                new StyleModel { StyleCode = "13D62:L78", StyleName = "San Martin Black Overdye Black" },
                new StyleModel { StyleCode = "13D30:L67", StyleName = "San Martin Blackdigo" },
                new StyleModel { StyleCode = "12D97/L02", StyleName = "Sense Mystic" },
                new StyleModel { StyleCode = "10D62:L67", StyleName = "Sky Black" },
                new StyleModel { StyleCode = "10D62:L81", StyleName = "Sky Black Overdye Black" },
                new StyleModel { StyleCode = "10D92:L67", StyleName = "Sky Dream Blue" },
                new StyleModel { StyleCode = "05D14:L55", StyleName = "St Barts Agean Blue" },
                new StyleModel { StyleCode = "12D50:L67", StyleName = "Stela Retro Blue" },
                new StyleModel { StyleCode = "13D4B:L02", StyleName = "Tango Vista Blue" },
                new StyleModel { StyleCode = "07D08:L67", StyleName = "Tehuacan Deep Indigo" },
                new StyleModel { StyleCode = "14D49:A02", StyleName = "Trinity Atlas" },
                new StyleModel { StyleCode = "35B62:A81B", StyleName = "Valley Black" },
                new StyleModel { StyleCode = "14B0C:A12", StyleName = "Victory Cold Blue" }
            };
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

                var data = await finishingService.GetInventoryMetersByStyle(styleCode);
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