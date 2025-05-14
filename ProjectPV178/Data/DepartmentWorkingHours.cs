using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPV178.Data
{
    [Owned]
    public class DepartmentWorkingHours
    {

        public int From;
        public int To;

        public static List<DepartmentWorkingHours> SampleWH()
        {
            var result = new List<DepartmentWorkingHours>();
            for (int i = 0; i < 7; i++)
            {
                var rand = new Random();

                var wh = new DepartmentWorkingHours { From = rand.Next(7, 10), To = rand.Next(15, 18) };
                result.Add(wh);
            }
            return result;
        }
        public override string ToString()
        {
            return $"{From.ToString()}:00 - {To.ToString()}:00";
        }
    }
}
