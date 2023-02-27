using BIZFEST_Event.Models;
using Microsoft.EntityFrameworkCore;

namespace BIZFEST_Event.DataAccess
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
        public DbSet<UserEvent> UserEvent { get; set;}
        public DbSet<UsersRegistration> UserRegistration { get; set;}
    }
}
