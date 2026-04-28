using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ProjectPV178.Data;
using ProjectPV178.Model;

namespace ProjectPV178.Database
{
    public class PeopleDBContext : DbContext
    {
        private string connectionString = @"server=(localdb)\MSSQLLocalDB;Initial Catalog=PeopleDB;Integrated Security = true";
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
            base.OnConfiguring(optionsBuilder);
        }

        public PeopleDBContext() : base()
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>()
                .OwnsMany(d => d.WorkingHours);

            modelBuilder.Entity<Department>()
                .Property(d => d.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<Department>()
                .HasMany(d => d.Doctors)
                .WithMany(doc => doc.Departments);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Patient);

           base.OnModelCreating(modelBuilder);
        }

    }
}
