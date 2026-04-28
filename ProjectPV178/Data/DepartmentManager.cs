using ProjectPV178.BussinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectPV178.Model;

namespace ProjectPV178.Data
{
    public class DepartmentManager
    {
        public static void SampleDepartments()
        {
            var rand = new Random();
            for (int i = 0; i < 4; i++)
            {
                int j = i;
                PeopleRepository.AddDepartment(j, Constants.DepartmentNames[rand.Next(Constants.DepartmentNames.Count)]);
            }
        }
        public static string Print(Department dep)
        {
            var doctorsString = "";
            foreach (var doc in dep.Doctors)
            {
                doctorsString += doc.Name + ", ";
            }

            var hoursString = "";
            DateTime dt = new DateTime();
            for (int i = 1; i < 6; i++)
            {
                var day = Enum.GetName(typeof(DayOfWeek), i);
                hoursString += $"{day} | {DepartmentWorkingHoursManager.Print(dep.WorkingHours.ToList()[i - 1])}\n";
            }
            return $"{dep.Name}\n" +
                $"{doctorsString}\n" +
                $"-----------------\n" +
                $"{hoursString}";
        }
    }
}
