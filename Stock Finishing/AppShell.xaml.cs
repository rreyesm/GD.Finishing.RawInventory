using Microsoft.Maui.Controls;
using Stock_Finishing.Models;
using Stock_Finishing.Services;
using Stock_Finishing.Views;

namespace Stock_Finishing
{
    public partial class AppShell : Shell
    {
        private readonly IFinishingService _service;
        public bool IsRawVisible { get; set; } = true;
        public bool IsProductionVisible { get; set; } = true;
        public bool IsReprocessVisible { get; set; } = true;

        public AppShell()
        {
            try
            {
                InitializeComponent();

                _service = App.Current.Services.GetService<IFinishingService>();
                BindingContext = this;

                Routing.RegisterRoute(nameof(LoginView), typeof(LoginView));
                Routing.RegisterRoute(nameof(StockView), typeof(StockView));
                Routing.RegisterRoute(nameof(StockProductionView), typeof(StockProductionView));
                Routing.RegisterRoute(nameof(StockReprocessView), typeof(StockReprocessView));
                Routing.RegisterRoute(nameof(ScanRollView), typeof(ScanRollView));
                Routing.RegisterRoute(nameof(ScanRollProductionView), typeof(ScanRollProductionView));
                Routing.RegisterRoute(nameof(ScanRollReprocessView), typeof(ScanRollReprocessView));
                Routing.RegisterRoute(nameof(ProfileView), typeof(ProfileView));

                //LoadTabs();

            }
            catch (Exception ex)
            {
                App.Current.Services.GetService<IMessageService>().ShowAlertAsync(ex.Message);
            }
        }

        //private async void LoadTabs()
        //{
        //    try
        //    {
        //        var resultModel = await _service.GetTabs();
        //        if (resultModel.IsSuccess)
        //        {
        //            var tabs = resultModel.Data;
        //            foreach (var tab in tabs)
        //            {
        //                switch (tab.TabName.ToLower())
        //                {
        //                    case "crudo":
        //                        IsRawVisible = tab.Show;
        //                        break;
        //                    case "rollos":
        //                        IsProductionVisible = tab.Show;
        //                        break;
        //                    case "reprocesos":
        //                        IsReprocessVisible = tab.Show;
        //                        break;
        //                }
        //            }
        //            OnPropertyChanged(nameof(IsRawVisible));
        //            OnPropertyChanged(nameof(IsProductionVisible));
        //            OnPropertyChanged(nameof(IsReprocessVisible));
        //        }
        //        else
        //        {
        //            await App.Current.Services.GetService<IMessageService>().ShowAlertAsync(resultModel.Message);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await App.Current.Services.GetService<IMessageService>().ShowAlertAsync(ex.Message);
        //    }
        //}
    }
}
