using ProjectPV178.BussinessLayer;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProjectPV178.Database;

namespace ProjectPV178.Model
{

    public class Department
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<Doctor> Doctors { get; } = [];
        public List<DepartmentWorkingHours> WorkingHours { get; set; }
        
    }
}
