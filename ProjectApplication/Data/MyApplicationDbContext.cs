using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectApplication.Entities;

namespace ProjectApplication.Data
{
    public class MyApplicationDbContext : IdentityDbContext<MyApplicationUser>
    {
        public MyApplicationDbContext(DbContextOptions<MyApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
    }
}
