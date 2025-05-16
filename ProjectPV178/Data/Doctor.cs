namespace ProjectPV178.Data
{
    public class Doctor : Person
    {
        public List<Department> Departments { get; } = [];
        public override bool IsDoctor => true;
    }
}
