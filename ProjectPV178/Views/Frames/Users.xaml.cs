using ProjectPV178.BussinessLayer;
using ProjectPV178.Data;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace ProjectPV178.Views
{
    /// <summary>
    /// Interaction logic for Users.xaml
    /// </summary>
    public partial class Users : Page
    {
        public ObservableCollection<Person> People { get; set; }
        public ObservableCollection<Department> Departments { get; set; }
        public Users()
        {
            InitializeComponent();

            People = new ObservableCollection<Person>
            (
                PeopleRepository.GetAllPeople().Result
            );
            DataGrid1.ItemsSource = People;
            DataContext = this;
        }
    }
}
