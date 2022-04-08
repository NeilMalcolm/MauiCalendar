using MauiCalendar.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MauiCalendar;

public class MainViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private DateState _state;
    public DateState State
    {
        get => _state;
        set
        {
            _state = value;
            OnPropertyChanged();
        }
    }

    public ICommand PrevMonthCommand { get; private set; }
    public ICommand PrevTwoMonthCommand { get; private set; }
    public ICommand CurrMonthCommand { get; private set; }
    public ICommand NextMonthCommand { get; private set; }
    public ICommand NextTwoMonthCommand { get; private set; }
    public ICommand OnDateSelectedCommand { get; private set; }


    private List<AppointmentViewModel> _allItems;
    public List<AppointmentViewModel> AllItems
    {
        get => _allItems;
        set
        {
            _allItems = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<AppointmentViewModel> _itemsForSelectedDay = new ObservableCollection<AppointmentViewModel>();
    public ObservableCollection<AppointmentViewModel> ItemsForSelectedDay
    {
        get => _itemsForSelectedDay;
        set
        {
            _itemsForSelectedDay = value;
            OnPropertyChanged();
        }
    }

    private string _selectedDayText;

    public string SelectedDayText
    {
        get => _selectedDayText;
        set 
        { 
            _selectedDayText = value; 
            OnPropertyChanged(); 
        }
    }

    public MainViewModel()
    {
        AllItems = new List<AppointmentViewModel>
        {
            new (new(2022,03,01), "Only one month 'til April fools!"),
            new (new(2022,04,01), "Be careful! April Fools today."),
            new (new(2022,04,01), "By the way, did you know it says gullible on the ceiling?"),
            new (new(2022,04,02), "April Fools is finally over!"),
            new (new(2022,05,01), "Can't believe it's been a month since April fools!"),
        }; 

        var initialDate = new DateOnly(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
        State = new DateState(initialDate);
        Initialize();
    }

    private void Initialize()
    {
        PrevTwoMonthCommand = new Command(() => State.GoBackMonths(2));
        PrevMonthCommand = new Command(() => State.GoBackMonths(1));
        CurrMonthCommand = new Command(State.GoToCurrentMonth);
        NextMonthCommand = new Command(() => State.GoForwardMonths(1));
        NextTwoMonthCommand = new Command(() => State.GoForwardMonths(2));
        OnDateSelectedCommand = new Command<DateOnly?>((date) => 
        {
            if (date is null) return;

            System.Diagnostics.Debug.WriteLine($"selected date '{date}'");
            SelectedDayText = date.Value.ToString("dd/MM/yy");
            ItemsForSelectedDay = new ObservableCollection<AppointmentViewModel>(AllItems.Where(x => x.Date == date));
        });
    }

    protected void OnPropertyChanged([CallerMemberName]string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}