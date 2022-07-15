using Microsoft.EntityFrameworkCore;
using SimpleRestAPI.Models;

namespace SimpleRestAPI.Services.Interface
{
    public interface IStudentsService
    {
        /*
         * Return all students
         */
        public DbSet<Students> Get();
        /*
         * Return all students with a specific name
         * Parameters:
         *  name: Name by which students are searched
         */
        public Students GetByName(string name);
        /*
         * Update a User
         * Parameters:
         *  students: the student to be modified (by id)
         */
        public void Put(Students students);
        /*
         * Add a User
         * Parameters:
         *  students: the student to be added
         */
        public Guid Post(Students students);
        /*
         * Delete a User
         * Parameters:
         *  students: the student who try to delete (only need a student who conteinst a id)
         */
        public void Delete(Students students);
    }
}
