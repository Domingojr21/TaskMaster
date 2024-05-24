using Microsoft.EntityFrameworkCore;
using TaskMaster.Core.Domain.Entities;

namespace TaskManager.Infrastructure.Persistence.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        public DbSet<Tasks> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Task
            modelBuilder.Entity<Tasks>().HasKey(t => t.Id);

            modelBuilder.Entity<Tasks>().ToTable("Task");

            modelBuilder.Entity<Tasks>()
                .Property(x => x.Title)
                .IsRequired();

            modelBuilder.Entity<Tasks>()
               .Property(x => x.DueDate)
               .IsRequired();
            #endregion
        }
    }
}
