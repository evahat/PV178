using Microsoft.EntityFrameworkCore;
using ProjectPV178.BussinessLayer;
using ProjectPV178.Database;
using ProjectPV178.Model;
using ProjectPV178.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using ProjectPV178.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjectPV178.Views.Pages
{
    /// <summary>
    /// Interaction logic for HomePageDoctor.xaml
    /// </summary>
    public partial class HomePageDoctor : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public string NameString { get; set; }
        private List<DateOnly> _myDaysOff;
        public List<DateOnly> MyDaysOff 
        {
            get => _myDaysOff;
            set
            {
                _myDaysOff = value;
                UpdateDay();
                OnPropertyChanged();
            }
        }

        private DateTime? _date;
        public DateTime? Date 
        {
            get => _date;
            set
            {
                _date = value;
                UpdateDay();
                OnPropertyChanged();
            }
        }
        private string _selectedDepLabel;
        public string SelectedDepLabel
        {
            get => _selectedDepLabel;
            set
            {
                _selectedDepLabel = value;
                OnPropertyChanged();
            }
        }
        private string _removeDepLabel;
        public string RemoveDepLabel
        {
            get => _removeDepLabel;
            set
            {
                _removeDepLabel = value;
                OnPropertyChanged();
            }
        }
        private ReservationSlot _upcomingSelected;
        public ReservationSlot UpcomingSelected
        {
            get => _upcomingSelected;
            set
            {
                _upcomingSelected = value;
                OnPropertyChanged();
            }
        }

        private string _reservationInfo;
        public string ReservationInfo
        {
            get => _reservationInfo;
            set
            {
                _reservationInfo = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Department> _departments;
        public ObservableCollection<Department> Departments
        {
            get => _departments;
            set
            {
                _departments = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Department> _mydepartments;
        public ObservableCollection<Department> MyDepartments
        {
            get => _mydepartments;
            set
            {
                _mydepartments = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<ReservationSlot> _day = new();
        public ObservableCollection<ReservationSlot> Day
        {
            get => _day;
            set
            {
                _day = value;
                OnPropertyChanged();
            }
        }
        private Department? _selectedDep;
        public Department? SelectedDep
        {
            get => _selectedDep;
            set
            {
                _selectedDep = value;
                OnPropertyChanged();
                UpdateSelectedDepartmentLabel();
                UpdateDay();
            }
        }
        private DateTime? _dayOffDate;
        public DateTime? DayOffDate
        {
            get => _dayOffDate;
            set
            {
                _dayOffDate = value;
                OnPropertyChanged();
                UpdateDay();
            }
        }
        private Department? _removeSelected;
        public Department? RemoveSelected
        {
            get => _removeSelected;
            set
            {
                _removeSelected = value;
                OnPropertyChanged();
                UpdateRemovedDepartmentLabel();
                UpdateDay();
            }
        }
        private Department? _assignDepSelected;
        public Department? AssignDepSelected
        {
            get => _assignDepSelected;
            set
            {
                _assignDepSelected = value;
                OnPropertyChanged();
                UpdateDay();
            }
        }
        public HomePageDoctor()
        {
            InitializeComponent();
            var curr = PeopleRepository.CurrentUser;
            NameString = curr.Name + " " + curr.Surname;
            Departments = new ObservableCollection<Department>
            (
                PeopleRepository.GetAllDepartments().Result
            );
            DataContext = this;
            InitializeAsync();
        }
        public void InitializeAsync()
        {
            DepartmentManager.SampleDepartments();
            using var db = new PeopleDBContext();
            var curr = db.Doctors.Include(d => d.Departments).FirstOrDefault(d => d.Username == ((Doctor)PeopleRepository.CurrentUser).Username);
            MyDepartments = new ObservableCollection<Department>
                (
                    curr.Departments
                );
            MyDaysOff = curr.DaysOff;
            db.SaveChanges();

        }

        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private void Button_Logout(object sender, RoutedEventArgs e)
        {
            PeopleRepository.CurrentUser = null;
            this.NavigationService?.Navigate(new LoginPage());
        }

        private void Button_Assign(object sender, RoutedEventArgs e)
        {
            if (AssignDepSelected is Department selectedDepartment)
            {
                using var db = new PeopleDBContext();
                var curr = db.Doctors.Include(d => d.Departments).FirstOrDefault(d => d.Username == PeopleRepository.CurrentUser.Username);
                var assigned = db.Departments.FirstOrDefault(d => d.Id == selectedDepartment.Id);
                if (!curr.Departments.Any(d => d.Name == selectedDepartment.Name))
                {
                    curr.Departments.Add(assigned);
                    assigned.Doctors.Add(curr);
                }
                db.SaveChanges();

                InitializeAsync();
            }
        }

        private void Button_Remove(object sender, RoutedEventArgs e)
        {
            if (RemoveSelected is Department selectedDepartment)
            {
                using (var db = new PeopleDBContext())
                {
                    var curr = db.Doctors.Include(d => d.Departments).SingleOrDefault(d => d.Username == PeopleRepository.CurrentUser.Username);
                    var assigned = db.Departments.Include(d => d.Doctors).SingleOrDefault(d => d.Id == selectedDepartment.Id);
                    curr.Departments.Remove(assigned);
                    db.SaveChanges();
                }

                InitializeAsync();
            }

        }

        private void Button_DayOff(object sender, RoutedEventArgs e)
        {
            if (DayOffDate.HasValue)
            {
                using (var db = new PeopleDBContext())
                {
                    var curr = db.Doctors.SingleOrDefault(d => d.Username == PeopleRepository.CurrentUser.Username);

                    DateOnly daOffDate = DateOnly.FromDateTime(DayOffDate.Value);
                    if (!curr.DaysOff.Contains(daOffDate))
                    {
                        curr.DaysOff.Add(daOffDate);
                        db.SaveChanges();
                        InitializeAsync();
                    }
                }
            }
        }
        private void UpdateDay()
        {
            ReservationInfo = "";
            using var db = new PeopleDBContext();

            db.SaveChanges();

            if (SelectedDep == null || Date == null)
            {
                Day = [];
                return;
            }
            if (SelectedDep.Doctors.All(d => d.DaysOff.Contains(DateOnly.FromDateTime((DateTime)Date))) || DateOnly.FromDateTime((DateTime)Date).DayOfWeek == DayOfWeek.Sunday || DateOnly.FromDateTime((DateTime)Date).DayOfWeek == DayOfWeek.Saturday)
            {
                ReservationInfo = "The ordination is closed on this day.";
                Day = [];
                return;
            }
            if (SelectedDep.Doctors.Any(d => d.DaysOff.Contains(DateOnly.FromDateTime((DateTime)Date))))
            {
                var docs = SelectedDep.Doctors.Where(d => d.DaysOff.Contains(DateOnly.FromDateTime((DateTime)Date))).Select(d => $"{d.Name} {d.Surname}");

                ReservationInfo = "Some doctors have a day off today:" + string.Join(", ", docs);
            }
            Day = new ObservableCollection<ReservationSlot>(
                DayReservation.GenerateDay(SelectedDep, DateOnly.FromDateTime((DateTime)Date))
            );

        }
        private void Button_RemoveUpcoming(object sender, RoutedEventArgs e)
        {
            if (UpcomingSelected is ReservationSlot reservation)
            {
                using var db = new PeopleDBContext();
                if (reservation.Reservation != null) {
                    var curr = db.Reservations.Include(r => r.Patient)
                        .FirstOrDefault(r => r.Patient.Username == reservation.Reservation.Patient.Username);
                    
                    db.Reservations.Remove(curr);
                    db.SaveChanges();
                    UpdateDay();
                }
            }
        }
        private void UpdateSelectedDepartmentLabel()
        {
            if (SelectedDep != null)
            {
                using var db = new PeopleDBContext();
                var fullDep = db.Departments.Include(d => d.Doctors)
                                .FirstOrDefault(d => d.Id == SelectedDep.Id);

                SelectedDepLabel = DepartmentManager.Print(fullDep);
            }
            else
            {
                SelectedDepLabel = string.Empty;
            }
        }
        private void UpdateRemovedDepartmentLabel()
        {
            if (RemoveSelected != null)
            {
                using var db = new PeopleDBContext();
                var fullDep = db.Departments.Include(d => d.Doctors)
                                .FirstOrDefault(d => d.Id == RemoveSelected.Id);

                RemoveDepLabel = DepartmentManager.Print(fullDep);
            }
            else
            {
                RemoveDepLabel = string.Empty;
            }
        }

    }

}
