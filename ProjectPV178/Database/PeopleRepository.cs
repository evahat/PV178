using Microsoft.EntityFrameworkCore;
using ProjectPV178.Data;
using ProjectPV178.Database;

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

        public static async Task<Person> GetPerson(string username)
        {
            using var db = new PeopleDBContext();

            return await db.People.FirstOrDefaultAsync(p=> p.Username == username);
        }
        public static async Task<List<Person>> GetAllPeople()
        {
            using var db = new PeopleDBContext();
            var a = db.People.ToList();
            return a;
        }
        public static async Task<List<Patient>> GetAllPatients()
        {
            using var db = new PeopleDBContext();
            var a = db.Patients.ToList();
            return a;
        }
        public static async Task<List<Doctor>> GetAllDoctors()
        {
            using var db = new PeopleDBContext();
            var a = db.Doctors.ToList();
            return a;
        }
        public static async Task<List<Department>> GetAllDepartments()
        {
            using var db = new PeopleDBContext();
            var a = db.Departments.ToList();
            return a;
        }
        public static Person CurrentUser { get; set; }
    }
}
