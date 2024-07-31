using Stock_Finishing.ViewModels;

namespace Stock_Finishing.Views;

public partial class StockReprocessView : ContentPage
{
    private StockReprocessViewModel _viewModel;

    public StockReprocessView()
	{
		InitializeComponent();
        _viewModel = new StockReprocessViewModel();
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadDataAsync();
    }
}