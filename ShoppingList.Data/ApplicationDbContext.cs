using ShoppingList.Data.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace ShoppingList.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<ListItem> ListItems { get; set; }
        public DbSet<LocalUser> LocalUsers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ListItem>().HasData(
                new ListItem()
                {
                    Id = 1,
                    Product = "Apple",
                    Quantity = 10,
                    IsBought = false
                },
                new ListItem()
                {
                    Id = 2,
                    Product = "Banana",
                    Quantity = 5,
                    IsBought = false
                });
        }
    }
}
