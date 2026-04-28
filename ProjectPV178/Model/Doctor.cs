namespace ProjectPV178.Model
{
    public class Doctor : Person
    {
        public List<Department> Departments { get; } = [];
        public override bool IsDoctor => true;
        public List<DateOnly> DaysOff { get; set; } = [];
    }
}
