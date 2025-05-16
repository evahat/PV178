using ProjectPV178.BussinessLayer;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProjectPV178.Database;

namespace ProjectPV178.Data
{

    public class Department
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<Doctor> Doctors { get; } = [];
        public List<DepartmentWorkingHours> WorkingHours { get; set; }

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
        public static void SampleDepartments()
        {
            var rand = new Random();
            for (int i = 0; i < 4; i++) 
            {
                int j = i;
                PeopleRepository.AddDepartment(j, DepartmentNames[rand.Next(DepartmentNames.Count)]);
            }
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
                hoursString += $"{day} | {this.WorkingHours.ToList()[i - 1].ToString()}\n";
            }
            return $"{this.Name}\n" +
                $"{doctorsString}\n" +
                $"-----------------\n" +
                $"{hoursString}";
        }
    }
}
