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
        public ObservableCollection<Person> People { get; set; }
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
        public string NameString { get; set; }


        public event PropertyChangedEventHandler? PropertyChanged;

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

        public HomePageDoctor()
        {
            InitializeComponent();
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
            MyDaysOffDataGrid.ItemsSource = curr.DaysOff;
            db.SaveChanges();

            NameString = curr.Name + " " + curr.Surname;
            Departments = new ObservableCollection<Department>
            (
                PeopleRepository.GetAllDepartments().Result
            );
            DepartmentComboBox.ItemsSource = MyDepartments;

            People = new ObservableCollection<Person>
            (
                PeopleRepository.GetAllPeople().Result
            );
            DataGrid1.ItemsSource = People;
        }

        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private void DepartmentComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DepartmentComboBox.SelectedItem is Department selectedDepartment)
            {
                using var db = new PeopleDBContext();
                var assigned = db.Departments.Include(d => d.Doctors).FirstOrDefault(d => d.Id == selectedDepartment.Id);

                DepLabel.Content = assigned.ToString();
                SelectedDep = selectedDepartment;
                db.SaveChanges();
            }
            UpdateDay();
        }
        private void MyDepartmentComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MyDepartmentComboBox.SelectedItem is Department selectedDepartment)
            {
                using var db = new PeopleDBContext();
                var assigned = db.Departments.Include(d => d.Doctors).FirstOrDefault(d => d.Id == selectedDepartment.Id);

                MyDepLabel.Content = assigned.ToString();
                db.SaveChanges();
            }
        }
        private void Button_Logout(object sender, RoutedEventArgs e)
        {
            PeopleRepository.CurrentUser = null;
            this.NavigationService?.Navigate(new LoginPage());
        }

        private void Button_Assign(object sender, RoutedEventArgs e)
        {
            if (AssignDep.SelectedItem is Department selectedDepartment)
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
            if (MyDepartmentComboBox.SelectedItem is Department selectedDepartment)
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
            if (DayOffDatePicker.SelectedDate.HasValue)
            {
                using (var db = new PeopleDBContext())
                {
                    var curr = db.Doctors.SingleOrDefault(d => d.Username == PeopleRepository.CurrentUser.Username);

                    DateOnly daOffDate = DateOnly.FromDateTime(DayOffDatePicker.SelectedDate.Value);
                    curr.DaysOff.Add(daOffDate);
                    db.SaveChanges();
                }
                InitializeAsync();
            }
        }
        private void DataGridUpcoming_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void SelectedDate_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            Date = SelectedDate.SelectedDate;
            UpdateDay();
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
            if (DataGridUpcoming.SelectedItem is ReservationSlot reservation)
            {
                using var db = new PeopleDBContext();
                var curr = db.Reservations.Include(r => r.Patient)
                    .FirstOrDefault(r => r.Patient.Username == reservation.Reservation.Patient.Username);
                db.Reservations.Remove(curr);
                db.SaveChanges();

                UpdateDay();
                //MyReservationInfo = "Removed";
            }
        }
    }

}
