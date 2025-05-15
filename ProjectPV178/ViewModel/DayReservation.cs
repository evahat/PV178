using ProjectPV178.BussinessLayer;
using ProjectPV178.Data;
using ProjectPV178.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPV178.ViewModel
{
    public class DayReservation
    {
        public ObservableCollection<Reservation> GenerateDay(Department department,Patient patient, DateOnly date) 
        {
            using var db = new PeopleDBContext();
            DayOfWeek day = date.DayOfWeek;
            if (day == DayOfWeek.Sunday | day == DayOfWeek.Saturday)
            {
                //throw execpotin
            }

            var filteredReservations = PeopleRepository.GetDateReservations(date, department);
            for (int i = department.WorkingHours[(int)day - 1].From; i < department.WorkingHours[(int)day - 1].To; i++)
            {

                var dateTime = new DateTime(date, new TimeOnly(i, 0, 0));

                if (filteredReservations.();
                {

                }
            }


            var info = new ObservableCollection<TimeReservation>();
            for (int i = 8; i < 16; i++)
            {
                info.Add(new TimeReservation(i, true));
            }
            return new Day(info);
        }
    }
}
