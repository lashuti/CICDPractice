using Microsoft.EntityFrameworkCore;

namespace SampleDockerCRUDApp
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql("Server=SampleDockerCRUD.db;Port=5432;Database=library;User Id=postgres;Password=postgres;Include Error Detail=true");
        }

        public DbSet<Book> Books => Set<Book>();
    }
}
