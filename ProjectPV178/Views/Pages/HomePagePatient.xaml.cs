using Microsoft.EntityFrameworkCore;
using ProjectPV178.BussinessLayer;
using ProjectPV178.Data;
using ProjectPV178.Database;
using ProjectPV178.Model;
using ProjectPV178.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        public ObservableCollection<string> DepartmentNameFromId { get; set; }
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
        public string NameString { get; set; }
        public ObservableCollection<ReservationSlot> Reservations { get; set; }

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
        public Department? SelectedDep { get; set; }
        public DateTime? Date { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
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

            Date = SelectedDate.SelectedDate;
            SelectedDep = (Department?)DepartmentComboBox.SelectedItem;
            UpdateDay();
        }
        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private void DepartmentComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DepartmentComboBox.SelectedItem is Department selectedDepartment)
            {
                DepLabel.Content = DepartmentManager.Print(selectedDepartment);
                SelectedDep = selectedDepartment;
            }
            UpdateDay();
        }

        private void SelectedDate_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            Date = SelectedDate.SelectedDate;
            UpdateDay();
        }
        private void UpdateDay()
        {
            using var db = new PeopleDBContext();

            MyReservations = new ObservableCollection<Reservation>
                (
                    db.Reservations.Where(r => r.Patient.Username == ((Patient)PeopleRepository.CurrentUser).Username)
                );
            db.SaveChanges();
            //DepName = new ObservableCollection<string> (MyReservations.Select(r => db.Departments.FirstOrDefault(d=> d.Id == r.DepartmentID).Name));
            if (SelectedDep == null || Date == null)
            {
                Day = [];
                return;
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
        private void DataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void DataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGrid1.SelectedItem is ReservationSlot selectedSlot)
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
            if (DataGrid1.SelectedItem is ReservationSlot selectedSlot)
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
                        DepartmentID = ((Department)DepartmentComboBox.SelectedItem).Id,
                        Date = DateOnly.FromDateTime((DateTime)Date),
                        Time = time,
                        Patient = curr
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
            if (DataGrid2.SelectedItem is Reservation reservation)
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
    }
}
