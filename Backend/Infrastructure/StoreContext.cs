using Microsoft.EntityFrameworkCore;
namespace Infrastructure

{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
    }

    public class Course
    {
         public Guid Id { get; set; }
    public string Title { get; set; }
     public String Description{ get; set; }
    public String Image { get; set; }
}
    }


