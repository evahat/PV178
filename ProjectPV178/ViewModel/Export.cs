using ProjectPV178.Database;
using ProjectPV178.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPV178.ViewModel
{
    public class Export
    {
        public static void ExportReservationsToTxt(string filePath, Patient curr)
        {
            var lines = new List<string>
            {
                "My Reservations",
                "-----------------",
                ""
            };
            using var db = new PeopleDBContext();
            foreach (var reservation in db.Reservations.Where(r=>r.Patient.Username==curr.Username))
            {
                lines.Add($"Department Name: {reservation.DepartmentName}");
                lines.Add($"Date         : {reservation.Date}");
                lines.Add($"Time         : {reservation.Time}");
                lines.Add(""); // Blank line between reservations
            }
            string filePathh = Path.Combine(filePath, "reservations.txt"); 
            File.WriteAllLines(filePath, lines);
        }

    }
}
