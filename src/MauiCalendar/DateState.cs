using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MauiCalendar
{
    public class DateState : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly DateOnly _initialDate;

        private DateOnly? _selectedDate;
        public DateOnly? SelectedDate
        {
            get => _selectedDate;
            set 
            { 
                _selectedDate = value;
                OnPropertyChanged();
            }
        }

        private DateOnly _displayDate;
        public DateOnly DisplayDate
        {
            get => _displayDate;
            set
            {
                _displayDate = value;
                OnPropertyChanged();
            }
        }

        public DateState(DateOnly initialDate)
            : this ()
        {
            _initialDate = DisplayDate = initialDate;
        }

        private DateState()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="num"></param>
        /// <exception cref="NotSupportedException">
        /// If num is less than or equal to 0.
        /// </exception>
        public void GoForwardMonths(int num)
        {
            if (num <= 0)
            {
                throw new NotSupportedException("Must be positive number");
            }

            DisplayDate = _initialDate.AddMonths(num);
        }

        public void GoToCurrentMonth()
        {
            DisplayDate = _initialDate;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="num"></param>
        /// <exception cref="NotSupportedException">
        /// If num is less than or equal to 0.
        /// </exception>
        public void GoBackMonths(int num)
        {
            if (num < 0)
            {
                throw new NotSupportedException("Must be positive number");
            }

            DisplayDate = _initialDate.AddMonths(-num);
        }

        protected void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
