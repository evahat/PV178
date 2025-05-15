using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPV178.Data
{
    public class Reservation
    {
        [Key]
        public required Department Department { get; set; }
        [Key]
        public required DateTime Date { get; set; }
        public required Patient Patient { get; set; }
    }
}
