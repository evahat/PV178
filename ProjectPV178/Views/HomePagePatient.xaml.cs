using Microsoft.EntityFrameworkCore;
using ProjectPV178.BussinessLayer;
using ProjectPV178.Data;
using ProjectPV178.Database;
using ProjectPV178.Model;
using ProjectPV178.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ProjectPV178.Views
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class HomePagePatient : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public Reservation MyReservationSelected { get; set; }
        public string NameString { get; set; }
        private string _exportInfo;
        public string ExportInfo
        {
            get => _exportInfo;
            set
            {
                _exportInfo = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Reservation> _myReservations;
        public ObservableCollection<Reservation> MyReservations
        {
            get => _myReservations;
            set
            {
                _myReservations = value;
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
        private DateTime? _date;
        public DateTime? Date 
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged();
                UpdateDay();
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
        private string _myreservationInfo;
        public string MyReservationInfo
        {
            get => _myreservationInfo;
            set
            {
                _myreservationInfo = value;
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
        private ReservationSlot? _selectedReservation;
        public ReservationSlot? SelectedReservation 
        {
            get => _selectedReservation;
            set
            {
                _selectedReservation = value;
                OnPropertyChanged();
                UpdateReservationInfo();
            }
        }
        public HomePagePatient()
        {
            InitializeComponent();
            DataContext = this;
            NameString = PeopleRepository.CurrentUser.Name + " " + PeopleRepository.CurrentUser.Surname;
            _ = InitializeAsync();

        }
        private async Task InitializeAsync()
        {
            DepartmentManager.SampleDepartments();

            Departments = new ObservableCollection<Department>
            (
                await PeopleRepository.GetAllDepartments()
            );

            UpdateDay();
        }
        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private async void UpdateSelectedDepartmentLabel()
        {
            if (SelectedDep != null)
            {
                using var db = new PeopleDBContext();
                var fullDep = db.Departments.Include(d => d.Doctors).Include(d=>d.WorkingHours)
                                .FirstOrDefault(d => d.Id == SelectedDep.Id);

                SelectedDepLabel = DepartmentManager.Print(fullDep);
                await db.SaveChangesAsync();
            }
            else
            {
                SelectedDepLabel = string.Empty;
            }
        }

        private async void UpdateDay()
        {
            ReservationInfo = "";
            using var db = new PeopleDBContext();
            MyReservations = new ObservableCollection<Reservation>
                (
                    db.Reservations.Where(r => r.Patient.Username == ((Patient)PeopleRepository.CurrentUser).Username)
                );
            await db.SaveChangesAsync();

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
        private void Button_Logout(object sender, RoutedEventArgs e)
        {
            PeopleRepository.CurrentUser = null;
            this.NavigationService?.Navigate(new LoginPage());
        }
        private void UpdateReservationInfo()
        {
            if (SelectedReservation is ReservationSlot selectedSlot)
            {
                var time = selectedSlot.Time;
                var isReserved = selectedSlot.Reservation;
                if (isReserved == null)
                {
                    ReservationInfo = "This slot is empty, you can reserve it.";
                }
                else { ReservationInfo = "This slot is already reserved."; }
            }
        }
        private async void Reserve(object sender, RoutedEventArgs e)
        {
            if (SelectedReservation is ReservationSlot selectedSlot)
            {
                var time = selectedSlot.Time;
                var isReserved = selectedSlot.Reservation;
                if (isReserved == null)
                {
                    using var db = new PeopleDBContext();
                    var curr = db.Patients
                        .FirstOrDefault(p => p.Username == PeopleRepository.CurrentUser.Username);
                    var reservation = new Reservation
                    {
                        DepartmentID = SelectedDep.Id,
                        Date = DateOnly.FromDateTime((DateTime)Date),
                        Time = time,
                        Patient = curr,
                        DepartmentName = SelectedDep.Name,
                    };
                    db.Reservations.Add(reservation);
                    await db.SaveChangesAsync();

                    UpdateDay();
                    ReservationInfo = "Reserved";
                }
            }
        }
        private async void Remove(object sender, RoutedEventArgs e)
        {
            if (MyReservationSelected is Reservation reservation)
            {
                using var db = new PeopleDBContext();
                var curr = db.Patients
                    .FirstOrDefault(p => p.Username == PeopleRepository.CurrentUser.Username);
                db.Reservations.Remove(reservation);
                await db.SaveChangesAsync();

                UpdateDay();
                MyReservationInfo = "Removed";
            }
        }

        private void ExportRes(object sender, RoutedEventArgs e)
        {
            try
            {
                using var db = new PeopleDBContext();
                var curr = db.Patients
                    .FirstOrDefault(p => p.Username == PeopleRepository.CurrentUser.Username);
                string downloadsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Downloads"
                );

                Export.ExportReservationsToTxt(downloadsPath, curr);

            }
            catch (Exception ex )
            {
                ExportInfo = "Couldnt export";
            }
        }
    }
}
