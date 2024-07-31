using Stock_Finishing.Models;
using Stock_Finishing.Services;
using Stock_Finishing.ViewModels;
using Stock_Finishing.Views;

namespace Stock_Finishing
{
    public partial class App : Application
    {
        public static string RestUrlLogin { get; private set; } = "http://192.168.7.4:93/api/user";
        public static string RestUrlFinishing { get; private set; } = "http://192.168.7.4:93/api/";

        //public static string RestUrlLogin { get; private set; } = "http://192.168.182.53:9090/api/user";
        //public static string RestUrlFinishing { get; private set; } = "http://192.168.182.53:9090/api/";
        public static UserModel UserModel { get; internal set; }
        public new static App Current => (App)Application.Current;
        public IServiceProvider Services { get; }

        public App()
        {
            try
            {
                var services = new ServiceCollection();
                Services = ConfigurationServices(services);

                InitializeComponent();

                MainPage = new LoginView();
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Test", ex.Message, "Ok", "Cancelar");
            }
        }

        private IServiceProvider ConfigurationServices(ServiceCollection services)
        {
            //Servicios
            services.AddSingleton<IMessageService, MessageService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IFinishingService, FinishingService>();

            //ViewModels
            services.AddTransient<LoginViewModel>();
            services.AddTransient<StockViewModel>();
            services.AddTransient<StockProductionViewModel>();
            services.AddTransient<StockReprocessViewModel>();
            services.AddTransient<ScanRollViewModel>();
            services.AddTransient<ScanRollProductionViewModel>();
            services.AddTransient<ScanRollReprocessViewModel>();

            //Views
            services.AddSingleton<LoginView>();
            services.AddSingleton<StockView>();
            services.AddSingleton<StockProductionView>();
            services.AddSingleton<StockReprocessView>();
            services.AddSingleton<ScanRollView>();
            services.AddSingleton<ScanRollProductionView>();
            services.AddSingleton<ScanRollReprocessView>();
            services.AddSingleton<ProfileView>();

            return services.BuildServiceProvider();
        }
    }
}
