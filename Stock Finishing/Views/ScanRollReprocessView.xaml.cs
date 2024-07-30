namespace Stock_Finishing.Views;

public partial class ScanRollReprocessView : ContentPage
{
	public ScanRollReprocessView()
	{
		InitializeComponent();
	}

    void entNumber_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is Entry)
        {
            Entry entry = (Entry)sender;
            if (entry.Text.Length == 1)
                entry.CursorPosition = entry.Text.Length;
        }
    }
}