using Microsoft.EntityFrameworkCore;
using GestCore.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<UserApp> UserApps { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aqui você pode configurar o esquema do banco de dados se necessário, como nomes de tabelas, chaves primárias, índices, relações, etc.
        modelBuilder.Entity<UserApp>(entity =>
        {
            entity.ToTable("UserApp");

            entity.HasKey(e => e.Id);

            // Configure as propriedades conforme as necessidades do seu banco de dados
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Password).IsRequired();
        });
    }
}
