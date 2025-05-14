using System.ComponentModel.DataAnnotations;

namespace ProjectPV178.Data
{

    public class Department
    {
        [Key]
        public required string Name { get; set; }
        public List<DepartmentWorkingHours> WorkingHours { get; set; }
        public required List<Doctor> Doctors { get; set; }

        public static List<string> DepartmentNames =
        [
            "Internal Medicine",
            "Cardiology",
            "Neurology",
            "Endocrinology",
            "Gastroenterology",
            "Pulmonology",
            "Nephrology",
            "Rheumatology",
            "Infectious Diseases",
            "General Surgery",
            "Orthopedic Surgery",
            "Neurosurgery",
            "Cardiothoracic Surgery",
            "Plastic and Reconstructive Surgery",
            "Urology",
            "Otolaryngology (ENT)",
            "Ophthalmology",
            "Emergency Department",
            "Intensive Care Unit (ICU)",
            "Neonatal Intensive Care Unit (NICU)",
            "Coronary Care Unit (CCU)",
            "Trauma Center",
            "Obstetrics and Gynecology (OB/GYN)",
            "Maternity Ward",
            "Pediatrics",
            "Neonatology",
            "Radiology",
            "Pathology",
            "Laboratory Services",
            "Pharmacy",
            "Anesthesiology",
            "Psychiatry",
            "Psychology",
            "Physical Therapy",
            "Occupational Therapy",
            "Speech Therapy",
            "Oncology",
            "Hematology",
            "Dermatology",
            "Geriatrics"
        ];
        public static List<Department> SampleDepartments()
        {
            var rand = new Random();
            return new List<Department>
            {
                new Department{ Name=DepartmentNames[rand.Next(0,DepartmentNames.Count)], WorkingHours=DepartmentWorkingHours.SampleWH(),Doctors=[] },
                new Department{ Name=DepartmentNames[rand.Next(0,DepartmentNames.Count)], WorkingHours=DepartmentWorkingHours.SampleWH(),Doctors=[] },
                new Department{ Name=DepartmentNames[rand.Next(0,DepartmentNames.Count)], WorkingHours=DepartmentWorkingHours.SampleWH(),Doctors=[] },
                new Department{ Name=DepartmentNames[rand.Next(0,DepartmentNames.Count)], WorkingHours=DepartmentWorkingHours.SampleWH(),Doctors=[] }
            };
        }
        public override string ToString()
        {
            var doctorsString = "";
            foreach (var doc in this.Doctors)
            {
                doctorsString += doc.Name + ", ";
            }
            
            var hoursString = "";
            DateTime dt = new DateTime();
            for (int i = 1; i < 6; i++)
            {
                var day = Enum.GetName(typeof(DayOfWeek), i);
                hoursString += $"{day} | {this.WorkingHours[i - 1].ToString()}\n";
            }
            return $"{this.Name}\n" +
                $"{doctorsString}\n" +
                $"-----------------\n" +
                $"{hoursString}";
        }
    }
}
