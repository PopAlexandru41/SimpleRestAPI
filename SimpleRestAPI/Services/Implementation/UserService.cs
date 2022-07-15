using Microsoft.EntityFrameworkCore;
using SimpleRestAPI.App_Data;
using SimpleRestAPI.Models;
using SimpleRestAPI.Services.Interface;

namespace SimpleRestAPI.Services.Implementation
{
    public class UserService :IUserService 
    {
        private readonly SimpleRestAPIDbContext _context;
        public UserService(SimpleRestAPIDbContext context)
        {
            _context = context;
        }

        public void Delete(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public DbSet<User> Get()
        {
            return _context.Users;
        }

        public User GetByName(string name)
        {
            return _context.Users.First(x=>x.Name==name);
        }

        public Guid Post(User user)
        {
            var itemS = new User()
            {
                Id = Guid.NewGuid(),
                Name = user.Name,
                Password = user.Password
            };
            _context.Entry(itemS).State = EntityState.Added;
            _context.SaveChanges();
            return itemS.Id;
        }

        public void Put(User user)
        {
            _context.Update(user);
            _context.SaveChanges();
        }
    }
}
