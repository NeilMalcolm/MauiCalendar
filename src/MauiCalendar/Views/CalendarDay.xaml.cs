using CalendarLib;
using System.Windows.Input;

namespace MauiCalendar.Views;

public partial class CalendarDay : ContentView
{
    private CalendarDate _thisDate;

    public static BindableProperty DateProperty =
        BindableProperty.Create
        (
            nameof(Date),
            typeof(DateOnly?),
            typeof(CalendarDay),
            null,
            BindingMode.Default
        );

    public DateOnly? Date
    {
        get => (DateOnly?)GetValue(DateProperty);
        set => SetValue(DateProperty, value);
    }

    public ICommand OnTappedCommand
    {
        get;
        set;
    }

	public CalendarDay()
	{
		InitializeComponent();
	}

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
    }

    public void Update(CalendarDate calendarDate, IEvent[] events)
    {
        Update(calendarDate);
        UpdateItems(events);
    }
    
    public void UpdateItems(IEvent[] events)
    {
        this.ItemsTodayLabel.Text = (events?.Length > 0 ? events.Length.ToString() : string.Empty);
        ItemsTodayLabel.TextColor = Colors.Gray;
    }

    public void Update(CalendarDate calendarDate)
    {
        _thisDate = calendarDate;
        Date = _thisDate.Date;

        IsEnabled = !_thisDate.IsDifferentMonth;

        this.DayLabel.Text = _thisDate.Date
            .Day
            .ToString();

        this.DayLabel.TextColor = _thisDate.IsDifferentMonth 
            ? Colors.Gray 
            : Color.FromHex("512bdf");
    }

    internal void Selected()
    {
        BackgroundColor = Color.FromHex("ad9fe3");
        ItemsTodayLabel.TextColor = Colors.White;
    }

    internal void Deselected()
    {
        BackgroundColor = Colors.Transparent;
        ItemsTodayLabel.TextColor = Colors.Gray;
    }

    private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        OnTappedCommand?.Execute(this);
    }
}
