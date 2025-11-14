using Microsoft.EntityFrameworkCore;
using SistemaGestionTalento.Domain.Entities;

namespace SistemaGestionTalento.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Colaborador> Colaboradores => Set<Colaborador>();
        public DbSet<Skill> Skills => Set<Skill>();
        public DbSet<Vacante> Vacantes => Set<Vacante>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar relaciones muchos a muchos explícitas si fuera necesario
            modelBuilder.Entity<Colaborador>()
                .HasMany(c => c.Skills)
                .WithMany(s => s.Colaboradores)
                .UsingEntity(j => j.ToTable("ColaboradorSkills"));

            modelBuilder.Entity<Vacante>()
                .HasMany(v => v.Skills)
                .WithMany(s => s.Vacantes)
                .UsingEntity(j => j.ToTable("VacanteSkills"));
        }
    }
}
