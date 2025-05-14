using ProjectPV178.BussinessLayer;
using ProjectPV178.Data;
using ProjectPV178.Database;
using System.Collections.ObjectModel;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;

namespace ProjectPV178.Views.Pages
{
    /// <summary>
    /// Interaction logic for HomePageDoctor.xaml
    /// </summary>
    public partial class HomePageDoctor : Page
    {
        public ObservableCollection<Person> People { get; set; }
        public ObservableCollection<Department> Departments { get; set; }
        public ObservableCollection<Department> MyDepartments { get; set; }
        public string NameString { get; set; }
        public HomePageDoctor()
        {
            Doctor curr = (Doctor) PeopleRepository.CurrentUser;
            InitializeComponent();
            NameString = curr.Name + " " + curr.Surname;

            Departments = new ObservableCollection<Department>
            (
                Department.SampleDepartments()
            );
            DepartmentComboBox.ItemsSource = Departments;

            People = new ObservableCollection<Person>
            (
                PeopleRepository.GetAllPeople().Result
            );
            DataGrid1.ItemsSource = People;
            
            MyDepartments = new ObservableCollection<Department>(curr.Departments);
            

            DataContext = this;
        }
        private void DepartmentComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DepartmentComboBox.SelectedItem is Department selectedDepartment)
            {
                DepLabel.Content = selectedDepartment.ToString();
            }
        }
        private void MyDepartmentComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MyDepartmentComboBox.SelectedItem is Department selectedDepartment)
            {
                MyDepLabel.Content = selectedDepartment.ToString();
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
                using (var db = new PeopleDBContext()){
                    Doctor curr = (Doctor)PeopleRepository.CurrentUser;
                    if (!curr.Departments.Any(d => d.Name == selectedDepartment.Name))
                    {
                        curr.Departments.Add(selectedDepartment);
                        db.Update(curr);
                        MyDepartments.Add(selectedDepartment);
                    }
                }
                
            }
        }
    }

}
