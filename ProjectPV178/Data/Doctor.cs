namespace ProjectPV178.Data
{
    public class Doctor : Person
    {
        public List<Department>? Departments { get; set; }
        public override bool IsDoctor => true;
    }
}
