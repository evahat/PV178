using ProjectPV178.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPV178.ViewModel
{
    public class ReservationSlot
    {
        public TimeOnly Time { get; set; }
        public Reservation? Reservation { get; set; }
        public bool IsReserved => Reservation != null;

    }
}

