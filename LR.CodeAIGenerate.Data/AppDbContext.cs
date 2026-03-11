using LR.CodeAIGenerate.Domain.Modelos;
using Microsoft.EntityFrameworkCore;

namespace LR.CodeAIGenerate.Data;

/// <summary>
/// DbContext para persistência em SQLite.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Construtor para uso sem DI (ex.: testes), usando SQLite com connection string padrão.
    /// </summary>
    public AppDbContext()
    {
    }

    public DbSet<Pessoa> Pessoas => Set<Pessoa>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlite("Data Source=app.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pessoa>(entity =>
        {
            entity.ToTable("Pessoas");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
            entity.Property(e => e.DataNascimento).IsRequired();
        });
    }
}
