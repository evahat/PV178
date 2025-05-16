using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPV178.Model
{
    [PrimaryKey(nameof(DepartmentID),nameof(Date),nameof(Time))]
    public class Reservation
    {
        public int DepartmentID { get; set; }
        public required string DepartmentName { get; set; }
        public required DateOnly Date { get; set; }
        public required TimeOnly Time { get; set; }
        public required Patient Patient { get; set; }
    }
}
