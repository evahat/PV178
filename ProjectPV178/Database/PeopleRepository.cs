using Microsoft.EntityFrameworkCore;
using ProjectPV178.Database;
using ProjectPV178.Model;
using ProjectPV178.Data;

namespace ProjectPV178.BussinessLayer
{
    public class PeopleRepository
    {
        public static async Task AddPerson(string username, string name, string surname, string password, bool isDoctor)
        {
            if (username == "" | name == "" | surname == "" | password == "")
            {
                throw new ArgumentNullException();
            }
            if (isDoctor)
            {
                using var db = new PeopleDBContext();
                var curr = new Doctor
                {
                    Username = username,
                    Name = name,
                    Surname = surname,
                    Password = password,
                };
                db.Doctors.Add(curr);
                db.People.Add(curr);
                CurrentUser = curr;

                await db.SaveChangesAsync();
            }
            else
            {
                using var db = new PeopleDBContext();
                var curr = new Patient
                {
                    Username = username,
                    Name = name,
                    Surname = surname,
                    Password = password,
                };
                db.Patients.Add(curr);
                db.People.Add(curr);
                CurrentUser = curr;
                await db.SaveChangesAsync();
            }
        }
        public static void AddDepartment(int id, string name)
        {
            using var db = new PeopleDBContext();
            if (db.Departments.Any(d => d.Id == id))
                return;
            var curr = new Department
            {
                Id = id,
                Name = name,
                WorkingHours = DepartmentWorkingHoursManager.SampleWH(),
            };
            db.Departments.Add(curr);

            db.SaveChanges();
        }
        public static List<Reservation> GetDateReservations(DateOnly date, Department department)
        {
            using var db = new PeopleDBContext();

            return db.Reservations.Include(r => r.Patient).Where(r => (r.Date == date & r.DepartmentID == department.Id)).ToList();
        }

        public static async Task<Person> GetPerson(string username)
        {
            using var db = new PeopleDBContext();

            return await db.People.FirstOrDefaultAsync(p => p.Username == username);
        }
        public static async Task<List<Department>> GetAllDepartments()
        {
            using var db = new PeopleDBContext();
            return db.Departments.Include(d => d.Doctors).ToList();
        }
        public static Person? CurrentUser
        {
            get; set;
        }
    }
}
