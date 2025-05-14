using ProjectPV178.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
                new Department("Cardiology",new DepartmentWorkingHours(8,14),[]),
            };
            Reservations = Days[0].Info;
            DataContext = this;
        }
    }
}
