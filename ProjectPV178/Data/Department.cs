using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPV178.Data
{
    public class Department
    {
        public string Name { get; set; }
        public DepartmentWorkingHours WorkingHours { get; set; }
        public List<Doctor> Doctors { get; set; }
        public Department(string name, DepartmentWorkingHours workingHours, List<Doctor> doctors)
        {
            Name = name;
            WorkingHours = workingHours;
            Doctors = doctors;
        }
    }
    public class DepartmentWorkingHours
    {
        public int From;
        public int To;
        public DepartmentWorkingHours(int from, int to)
        {
            From = from; To = to;
        }
    }
    public class Doctor
    {
        public string Name { get; set; }
        public Doctor(string name)
        {
            Name = name;
        }
    }
}
