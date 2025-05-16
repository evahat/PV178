using ProjectPV178.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPV178.Data
{
    public class DepartmentWorkingHoursManager
    {
        public static List<DepartmentWorkingHours> SampleWH()
        {
            var result = new List<DepartmentWorkingHours>();
            var rand = new Random();
            for (int i = 0; i < 7; i++)
            {

                var wh = new DepartmentWorkingHours { From = rand.Next(7, 10), To = rand.Next(15, 18) };
                result.Add(wh);
            }
            return result;
        }
        public string Print(DepartmentWorkingHours dwh)
        {
            return $"{dwh.From.ToString()}:00 - {dwh.To.ToString()}:00";
        }
    }
}
