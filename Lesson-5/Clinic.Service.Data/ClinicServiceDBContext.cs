using Microsoft.EntityFrameworkCore;

namespace Clinic.Service.Data;

public class ClinicServiceDBContext : DbContext
{

    public DbSet<Client> Clients { get; set; }
    public DbSet<Pet> Pets { get; set; }
    public DbSet<Consultation> Consultations { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<AccountSession> AccountSessions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Consultation>()
            .HasOne(pet => pet.Pet)
            .WithMany(b => b.Consultations)
            .HasForeignKey(p => p.PetId)
            .OnDelete(DeleteBehavior.NoAction);
    }

    public ClinicServiceDBContext(DbContextOptions options) : base(options)
    {
    }
}
