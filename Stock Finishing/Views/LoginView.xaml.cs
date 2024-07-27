using Stock_Finishing.ViewModels;

namespace Stock_Finishing.Views;

public partial class LoginView : ContentPage
{
	public LoginView()
	{
		InitializeComponent();
        BindingContext = App.Current.Services.GetService<LoginViewModel>();
    }

    void OnTextChanged(object sender, EventArgs e)
    {
        btnLogin.IsEnabled = !string.IsNullOrEmpty(usernameEntry.Text) && !string.IsNullOrEmpty(passwordEntry.Text);
    }
}