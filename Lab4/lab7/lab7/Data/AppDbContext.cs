using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<LinkModel> Links { get; set; }
    public DbSet<CommentModel> Comments { get; set; } 

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(LocalDB)\\MSSQLLocalDB;Database=UWSR;Trusted_Connection=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CommentModel>()
            .HasOne(c => c.Link)
            .WithMany(l => l.Comments)
            .HasForeignKey(c => c.LinkId)
            .OnDelete(DeleteBehavior.Cascade); 
    }
}
