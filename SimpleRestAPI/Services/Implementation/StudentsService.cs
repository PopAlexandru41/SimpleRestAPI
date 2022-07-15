using Microsoft.EntityFrameworkCore;
using SimpleRestAPI.App_Data;
using SimpleRestAPI.Models;
using SimpleRestAPI.Services.Interface;

namespace SimpleRestAPI.Services.Implementation
{
    public class StudentsService : IStudentsService
    {
        private readonly SimpleRestAPIDbContext _context;
        public StudentsService(SimpleRestAPIDbContext context)
        {
            _context = context;
        }
        public void Delete(Students students)
        {
            _context.Students.Remove(students);
            _context.SaveChanges();
        }

        public DbSet<Students> Get()
        {
            return _context.Students;
        }

        public Students GetByName(string name)
        {
            return _context.Students.First(x=>x.Name==name);
        }

        public Guid Post(Students students)
        {
            var itemS = new Students()
            {
                Id = Guid.NewGuid(),
                Name = students.Name,
                Age = students.Age,
                FinalNote = students.FinalNote,
                Observation = students.Observation
            };
            _context.Entry(itemS).State = EntityState.Added;
            _context.SaveChanges();
            return itemS.Id;
        }

        public void Put(Students students) 
        {
            _context.Update(students);
            _context.SaveChanges();
        }
    }
}
