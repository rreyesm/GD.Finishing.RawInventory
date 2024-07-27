using Stock_Finishing.Services;
using Stock_Finishing.Views;

namespace Stock_Finishing
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            try
            {
                InitializeComponent();
                Routing.RegisterRoute(nameof(LoginView), typeof(LoginView));
                Routing.RegisterRoute(nameof(StockView), typeof(StockView));
                Routing.RegisterRoute(nameof(ScanRollView), typeof(ScanRollView));
                Routing.RegisterRoute(nameof(ScanRollProductionView), typeof(ScanRollProductionView));
                Routing.RegisterRoute(nameof(ScanRollReprocessView), typeof(ScanRollReprocessView));
                Routing.RegisterRoute(nameof(ProfileView), typeof(ProfileView));
            }
            catch (Exception ex)
            {
                App.Current.Services.GetService<IMessageService>().ShowAlertAsync(ex.Message);
            }
        }
    }
}
