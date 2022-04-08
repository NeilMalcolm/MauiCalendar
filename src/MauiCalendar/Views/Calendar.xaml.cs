using CalendarLib;
using System.Windows.Input;

namespace MauiCalendar.Views;

public partial class Calendar : Grid
{
	public static BindableProperty SelectedDateProperty =
		BindableProperty.Create
		(
			nameof(SelectedDate),
			typeof(DateOnly?),
			typeof(Calendar),
			null,
			BindingMode.TwoWay,
			propertyChanged: OnSelectedDateChanged
		);

	public DateOnly? SelectedDate
    {
		get => (DateOnly?)GetValue(SelectedDateProperty);
		set => SetValue(SelectedDateProperty, value);
    }

	public static BindableProperty DisplayDateProperty =
		BindableProperty.Create
		(
			nameof(DisplayDate),
			typeof(DateOnly),
			typeof(Calendar),
			DateOnly.FromDateTime(DateTime.Today),
			propertyChanged: OnDisplayDateChanged
		);

	public DateOnly DisplayDate
	{
		get => (DateOnly)GetValue(DisplayDateProperty);
		set => SetValue(DisplayDateProperty, value);
	}

	private ICommand Calendar_OnDateSelectedCommand
	{ 
		get; 
		set; 
	}

	public static BindableProperty OnDateSelectedCommandProperty =
		BindableProperty.Create
		(
			nameof(OnDateSelectedCommand),
			typeof(ICommand),
			typeof(Calendar),
			null
		);

	public ICommand OnDateSelectedCommand
	{
		get => (ICommand)GetValue(OnDateSelectedCommandProperty);
		set => SetValue(OnDateSelectedCommandProperty, value);
	}

	public static BindableProperty ItemsSourceProperty =
		BindableProperty.Create
		(
			nameof(ItemsSource),
			typeof(IEnumerable<IEvent>),
			typeof(Calendar),
			null,
			propertyChanged: OnItemsSourceChanged
		);

	public IEnumerable<IEvent> ItemsSource
	{
		get => (IEnumerable<IEvent>)GetValue(ItemsSourceProperty);
		set => SetValue(ItemsSourceProperty, value);
	}

	public Calendar()
	{
		InitializeComponent();
		Calendar_OnDateSelectedCommand = new Command<CalendarDay>((day) =>
		{
			// handle state (selected day)
			// bubble up to ViewModel.

			(Children.FirstOrDefault(x => x is CalendarDay s && s.Date == SelectedDate) as CalendarDay)?.Deselected();
			day.Selected();
			SelectedDate = day.Date;
			OnDateSelectedCommand?.Execute(SelectedDate);
		});
		Build(DateBuilder.FromMonth(DisplayDate));
	}

	protected static void OnSelectedDateChanged(BindableObject sender, object oldValue, object newValue)
    {

    }

	protected static void OnDisplayDateChanged(BindableObject sender, object oldValue, object newValue)
    {
		if (sender is not Calendar calendar || newValue is not DateOnly newSelectedDate || oldValue == newValue)
        {
			return;
        }

		var calendarModel = DateBuilder.FromMonth(newSelectedDate);
		calendar.Build(calendarModel);
	}
	
	protected static void OnItemsSourceChanged(BindableObject sender, object oldValue, object newValue)
    {
		if (sender is not Calendar calendar || newValue is not IEnumerable<IEvent> collection || oldValue == newValue)
        {
			return;
        }

		for (int i = 0; i < calendar.Children.Count; i++)
		{
			var calendarDay = (calendar.Children[i] as CalendarDay);
			var itemsForDay = calendar.ItemsSource?.Where(x => x.Date == calendarDay.Date).ToArray();
			calendarDay.UpdateItems(itemsForDay);
		}
	}

	private void Build(CalendarDate[] calendarModel)
    {
		for (int i = 0; i < calendarModel.Length; i++)
        {
			var day = (Children[i] as CalendarDay);
			var itemsForDay = ItemsSource?.Where(x => x.Date == calendarModel[i].Date).ToArray();
			day.Update(calendarModel[i], itemsForDay);
			day.OnTappedCommand = Calendar_OnDateSelectedCommand;

			if (day.Date == SelectedDate)
			{
				day.Selected();
			}
			else
            {
				day.Deselected();
			}
		}
	}
}