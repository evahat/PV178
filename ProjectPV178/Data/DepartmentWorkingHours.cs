using Microsoft.EntityFrameworkCore;

namespace ProjectPV178.Data
{
    [Owned]
    public class DepartmentWorkingHours
    {
        public int From { get; set; }
        public int To { get; set; }

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
        public override string ToString()
        {
            return $"{From.ToString()}:00 - {To.ToString()}:00";
        }
    }
}
