using ProjectPV178.BussinessLayer;
using ProjectPV178.Database;
using ProjectPV178.Model;
using ProjectPV178.Views;
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
        public static List<ReservationSlot> GenerateDay(Department department, DateOnly date) 
        {
            using var db = new PeopleDBContext();
            DayOfWeek day = date.DayOfWeek;

            var info = new ObservableCollection<Reservation?>();
            var filteredReservations = PeopleRepository.GetDateReservations(date, department);

            var timeSlots = new List<TimeOnly>();
            for (int i = department.WorkingHours[(int)day - 1].From; i <= department.WorkingHours[(int)day - 1].To; i++)
            {
                timeSlots.Add(new TimeOnly(i,0,0));
            }

            var reservationSlots = timeSlots.Select(time => new ReservationSlot
            {
                Time = time,
                Reservation = filteredReservations.FirstOrDefault(r => r.Time == time)
            }).ToList();

            return reservationSlots;
        }
    }
}
