using System.Xml.Linq;

namespace Stock_Finishing.Views;

public partial class ProfileView : ContentPage
{
    public string Username { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }

    public ProfileView()
	{
		InitializeComponent();
        LoadUserData();
        BindingContext = this;
	}

    private async void OnLogoutButtonClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new LoginView();

        await Task.Delay(500);

        Application.Current.MainPage = new NavigationPage(new LoginView())
        {
            BarBackgroundColor = Color.FromArgb("151e3d"),
            BarTextColor = Color.FromArgb("151e3d"),
            BackgroundColor = Color.FromArgb("151e3d")
        };
    }

    private void LoadUserData()
    {
        Username = App.UserModel.UserName;
        Name = App.UserModel.Name;
        LastName = App.UserModel.LastName ?? "GD";
    }
}