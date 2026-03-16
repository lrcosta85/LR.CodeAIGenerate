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
    public DbSet<LR.CodeAIGenerate.Domain.Modelos.Endereco> Enderecos => Set<LR.CodeAIGenerate.Domain.Modelos.Endereco>();

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
            entity.Property<DateTime>("DataCadastro").IsRequired();
            entity.Property<string>("CPF").IsRequired().HasMaxLength(11);
        });

        modelBuilder.Entity<LR.CodeAIGenerate.Domain.Modelos.Endereco>(entity =>
        {
            entity.ToTable("Enderecos");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CEP).IsRequired().HasMaxLength(8);
            entity.Property(e => e.Logradouro).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Complemento).HasMaxLength(200);
            entity.Property(e => e.Bairro).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Cidade).IsRequired().HasMaxLength(100);
            entity.Property(e => e.UF).IsRequired().HasMaxLength(2);

            // One-to-one relationship: Pessoa (principal) 1 -> 0/1 Endereco (dependent)
            entity.HasOne<LR.CodeAIGenerate.Domain.Modelos.Pessoa>()
                  .WithOne(p => p.Endereco)
                  .HasForeignKey<LR.CodeAIGenerate.Domain.Modelos.Endereco>(e => e.IdPessoa)
                  .IsRequired();
        });
    }
}
