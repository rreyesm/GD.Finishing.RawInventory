namespace Stock_Finishing.Views;

public partial class ScanRollView : ContentPage
{
	public ScanRollView()
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