namespace MauiCalendar.Models;

public class AppointmentViewModel : IEvent
{
    public DateOnly Date { get; set; }
    public string Text { get; private set; }

    public AppointmentViewModel(DateOnly date, string text)
        => (Date, Text) = (date, text);
}