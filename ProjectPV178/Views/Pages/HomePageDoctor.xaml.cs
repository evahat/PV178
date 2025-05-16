using Microsoft.EntityFrameworkCore;
using ProjectPV178.BussinessLayer;
using ProjectPV178.Data;
using ProjectPV178.Database;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace ProjectPV178.Views.Pages
{
    /// <summary>
    /// Interaction logic for HomePageDoctor.xaml
    /// </summary>
    public partial class HomePageDoctor : Page,INotifyPropertyChanged
    {
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
        public HomePageDoctor()
        {
            InitializeComponent();
            DataContext = this;
            InitializeAsync();
        }
        public void InitializeAsync()
        { 
            Department.SampleDepartments();
            using var db = new PeopleDBContext();
            var curr = db.Doctors.Include(d=>d.Departments).FirstOrDefault(d => d.Username == ((Doctor)PeopleRepository.CurrentUser).Username);
            MyDepartments = new ObservableCollection<Department>
                (
                    curr.Departments
                );
            db.SaveChanges();

            NameString = curr.Name + " " + curr.Surname;
            Departments = new ObservableCollection<Department>
            (
                PeopleRepository.GetAllDepartments().Result
            );
            DepartmentComboBox.ItemsSource = Departments;

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
                db.SaveChanges();
            }
        }
        private void MyDepartmentComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MyDepartmentComboBox.SelectedItem is Department selectedDepartment)
            {
                using var db = new PeopleDBContext();
                var assigned = db.Departments.Include(d=>d.Doctors).FirstOrDefault(d => d.Id == selectedDepartment.Id);

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
                using var db = new PeopleDBContext() ;
                var curr = db.Doctors.Include(d=>d.Departments).FirstOrDefault(d=>d.Username == PeopleRepository.CurrentUser.Username);
                var assigned = db.Departments.FirstOrDefault(d=>d.Id == selectedDepartment.Id);
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
    }

}
