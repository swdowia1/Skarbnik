using Microsoft.EntityFrameworkCore;

namespace Skarbnik.DB
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Uczen> Uczniowie => Set<Uczen>();

        public DbSet<Wydatek> Wydatki => Set<Wydatek>();

        public DbSet<UczenWydatek> UczenWydatki => Set<UczenWydatek>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UczenWydatek>()
                .HasOne(uw => uw.Uczen)
                .WithMany(u => u.UczenWydatki)
                .HasForeignKey(uw => uw.UczenId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UczenWydatek>()
                .HasOne(uw => uw.Wydatek)
                .WithMany(w => w.UczenWydatki)
                .HasForeignKey(uw => uw.WydatekId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UczenWydatek>()
                .HasIndex(uw => new { uw.UczenId, uw.WydatekId })
                .IsUnique();

            modelBuilder.Entity<Wydatek>()
                .Property(w => w.Kwota)
                .HasPrecision(18, 2);

           
        }
    }
}
