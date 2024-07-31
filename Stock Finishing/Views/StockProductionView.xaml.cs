using Stock_Finishing.ViewModels;

namespace Stock_Finishing.Views;

public partial class StockProductionView : ContentPage
{
    private StockProductionViewModel _viewModel;

    public StockProductionView()
	{
		InitializeComponent();
        _viewModel = new StockProductionViewModel();
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadDataAsync();
    }
}