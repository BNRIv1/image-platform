using ImagePlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace ImagePlatform.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options)
        {
            
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Category>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<User>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Post>()
                .HasOne(p => p.Author)
                .WithMany(p => p.Posts)
                .HasForeignKey(p => p.AuthorId);

            modelBuilder.Entity<Post>()
                .HasOne(p => p.Category)
                .WithMany(p => p.Posts)
                .HasForeignKey(p => p.CategoryId);
        }
    }
}
