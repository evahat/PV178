using ProjectPV178.BussinessLayer;
using ProjectPV178.Data;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace ProjectPV178.Views
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class HomePagePatient : Page
    {
        public ObservableCollection<Department> Departments { get; set; }
        public string NameString { get; set; }
        public ObservableCollection<TimeReservation> Reservations { get; set; }
        public ObservableCollection<Day> Days { get; set; }
        public DateTime? Date { get; set; }
        public HomePagePatient()
        {
            InitializeComponent();
            NameString = PeopleRepository.CurrentUser.Name + " " + PeopleRepository.CurrentUser.Surname;

            Departments = new ObservableCollection<Department>
            (
                Department.SampleDepartments()
            );

            DepartmentComboBox.ItemsSource = Departments;

            Days = new ObservableCollection<Day>
            {
                Day.SampleDay()
            };
            Reservations = Days[0].Info;
            Date = SelectedDate.SelectedDate;

            DataContext = this;
        }
        private void DepartmentComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DepartmentComboBox.SelectedItem is Department selectedDepartment)
            {
                DepLabel.Content = selectedDepartment.ToString();
            }
        }
        private void Button_Logout(object sender, RoutedEventArgs e)
        {
            PeopleRepository.CurrentUser = null;
            this.NavigationService?.Navigate(new LoginPage());
        }
    }
}
