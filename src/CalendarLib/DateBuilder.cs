namespace CalendarLib;

public static class DateBuilder
{
    private const int DayArrayLength = 42;

    public static CalendarDate[] FromMonth(DateOnly date)
    {
        CalendarDate[] allDates = new CalendarDate[DayArrayLength];

        var firstOfMonth = new DateOnly(date.Year, date.Month, 1);
        int index = firstOfMonth.DayOfWeek.GetIndex();

        // if the first day of this month is not the first position,
        // fill the prior days with previous month days.
        if (index > 0)
        {
            var prevMonthIndex = index - 1;

            while (prevMonthIndex >= 0)
            {
                allDates[prevMonthIndex] = new CalendarDate
                (
                    firstOfMonth.AddDays(-(index - prevMonthIndex)),
                    true
                );

                prevMonthIndex--;
            }
        }

        int daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
        for (int day = 1; day <= daysInMonth; day++)
        {
            allDates[index] = new CalendarDate(new DateOnly(date.Year, date.Month, day));
            index++;
        }

        while (index < DayArrayLength)
        {
            allDates[index] = new CalendarDate
            (
                allDates[index - 1].Date.AddDays(1), 
                true
            );

            index++;
        }

        return allDates;
    }

}

internal static class DayOfWeekEx
{
    internal static int GetIndex(this DayOfWeek dayOfWeek) => dayOfWeek switch
    {
        DayOfWeek.Tuesday => 1,
        DayOfWeek.Wednesday => 2,
        DayOfWeek.Thursday => 3,
        DayOfWeek.Friday => 4,
        DayOfWeek.Saturday => 5,
        DayOfWeek.Sunday => 6,
        _ or DayOfWeek.Monday => 0,
    };
}
