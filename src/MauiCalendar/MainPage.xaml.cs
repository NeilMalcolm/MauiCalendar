namespace MauiCalendar;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
		BindingContext = new MainViewModel();
        SetViewsGridLayoutOnOrientationChange(DeviceDisplay.MainDisplayInfo.Orientation);
        DeviceDisplay.MainDisplayInfoChanged += DeviceDisplay_MainDisplayInfoChanged;
	}

    private void DeviceDisplay_MainDisplayInfoChanged(object sender, DisplayInfoChangedEventArgs e)
    {
        SetViewsGridLayoutOnOrientationChange(e.DisplayInfo.Orientation);
    }

    private void SetViewsGridLayoutOnOrientationChange(DisplayOrientation orientation)
    {
        if (orientation == DisplayOrientation.Portrait)
        {
            MainGrid.ColumnDefinitions = new ColumnDefinitionCollection { new (){ Width = GridLength.Star } };
            MainGrid.RowDefinitions = new RowDefinitionCollection { new (){ Height = GridLength.Star }, new (){ Height = GridLength.Star } };

            Grid.SetColumn(CalendarStack, 0);
            Grid.SetRow(CalendarStack, 0);

            Grid.SetColumn(DetailsStack, 0);
            Grid.SetRow(DetailsStack, 1);

            return;
        }

        MainGrid.RowDefinitions = new RowDefinitionCollection { new() { Height = GridLength.Star } };
        MainGrid.ColumnDefinitions = new ColumnDefinitionCollection { new() { Width = GridLength.Star }, new() { Width = GridLength.Star } };

        Grid.SetColumn(CalendarStack, 0);
        Grid.SetRow(CalendarStack, 0);

        Grid.SetColumn(DetailsStack, 1);
        Grid.SetRow(DetailsStack, 0);
    }
}

