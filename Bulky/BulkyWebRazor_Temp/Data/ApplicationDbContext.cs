using BulkyWebRazor_Temp.Model;
using Microsoft.EntityFrameworkCore;

namespace BulkyWebRazor_Temp.Data
{
    public class ApplicationDbContext : DbContext
    {   //1
        //we have to pass connection string in appsetting.json through DbContext
        //typing CTOR
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        //2
        //typing prop , DbSet --> EF Rule, <Category> --> models Category.cs, Categories --> table name will be created in database
        //after create DbSet like under this, run add-migration in Package Manager Console
        public DbSet<Category> Categories { get; set; }

        //4 insert data which code in backend, and run add-migration SeedCategoryTable
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
                new Category { Id = 2, Name = "Scifi", DisplayOrder = 2 },
                new Category { Id = 3, Name = "History", DisplayOrder = 3 }
                );
        }
    }
}
