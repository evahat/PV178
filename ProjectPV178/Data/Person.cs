using System.ComponentModel.DataAnnotations;

namespace ProjectPV178.Data
{
    public class Person
    {
        [Key]
        public required string Username { get; set; }

        public required string Name { get; set; }

        public required string Surname { get; set; }

        public required string Password { get; set; }
        public virtual bool IsDoctor => false;
    }
}
