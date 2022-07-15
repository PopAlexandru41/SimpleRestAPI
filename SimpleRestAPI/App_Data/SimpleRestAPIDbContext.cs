

using Microsoft.EntityFrameworkCore;
using SimpleRestAPI.Models;

namespace SimpleRestAPI.App_Data
{
    public class SimpleRestAPIDbContext: DbContext
    {
        public SimpleRestAPIDbContext(DbContextOptions<SimpleRestAPIDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Students> Students { get; set; }

    }
}
