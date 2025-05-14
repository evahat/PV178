using ProjectPV178.Data;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using ProjectPV178.Database;
using ProjectPV178.BussinessLayer;

namespace ProjectPV178.Views
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public ObservableCollection<Department> Departments { get; set; }
        public string Username { get; set; }
        public HomePage()
        {
            InitializeComponent();
            Username = PeopleRepository.CurrentUser.Name;

            Departments = new ObservableCollection<Department>
            (
                Department.SampleDepartments()
            );

            DepartmentComboBox.ItemsSource = Departments;
            DataContext = this;
        }
        private void DepartmentComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DepartmentComboBox.SelectedItem is Department selectedDepartment)
            {
                DepLabel.Content = selectedDepartment.ToString();
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Table.Content = new MyReservations();
        }
        private void Button_Click_Users(object sender, RoutedEventArgs e)
        {
            Table.Content = new Users();
        }
    }
}
