using Microsoft.EntityFrameworkCore;
using SimpleRestAPI.Models;

namespace SimpleRestAPI.Services.Interface
{
    public interface IUserService
    {
        /*
        * Return all Users
        */
        public DbSet<User> Get();
        /*
         * Return all Users with a specific name
         * Parameters:
         *  name: Name by which users are searched
         */
        public User GetByName(string name);
        /*
         * Update a User
         * Parameters:
         *  user: the user to be modified (search by id)
         */
        public void Put(User user);
        /*
         * Add a User
         * Parameters:
         *  user: the user to be added
         */
        public Guid Post(User user);
        /*
         * Delete a User
         * Parameters:
         *  user: the user who try to delete (only need a user who conteinst a id)
         */
        public void Delete(User user);
    }
}
