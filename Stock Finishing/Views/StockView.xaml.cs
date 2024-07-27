using Stock_Finishing.ViewModels;

namespace Stock_Finishing.Views;

public partial class StockView : ContentPage
{
	private StockViewModel _viewModel;
	public StockView()
	{
		InitializeComponent();
		_viewModel = new StockViewModel();
		BindingContext = _viewModel;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadDataAsync();
    }
}