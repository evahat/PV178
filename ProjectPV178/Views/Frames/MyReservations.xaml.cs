using ProjectPV178.Data;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace ProjectPV178.Views
{
    /// <summary>
    /// Interaction logic for MyReservations.xaml
    /// </summary>
    public partial class MyReservations : Page
    {
        public ObservableCollection<Day> Days { get; set; }
        public ObservableCollection<Department> Departments { get; set; }
        public ObservableCollection<TimeReservation> Reservations { get; set; }
        public MyReservations()
        {
            InitializeComponent();

            Days = new ObservableCollection<Day>
            {
                Day.SampleDay()
            };
            Departments = new ObservableCollection<Department>
            {
                //new Department("Cardiology",DepartmentWorkingHours.SampleWH(),[]),
            };
            Reservations = Days[0].Info;
            DataContext = this;
        }
    }
}
