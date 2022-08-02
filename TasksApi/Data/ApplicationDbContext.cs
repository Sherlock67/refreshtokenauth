using Microsoft.EntityFrameworkCore;
using TasksApi.Model;

namespace TasksApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }
        public  DbSet<RefreshTokens> RefreshTokens { get; set; }
        public  DbSet<Model.Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
