namespace ProjectPV178.Data
{
    public class Patient : Person
    {
        public override bool IsDoctor => false;
        public List<Reservation> Reservations { get; set; } = [];
    }
}
