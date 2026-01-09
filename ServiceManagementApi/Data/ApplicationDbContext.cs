using Microsoft.EntityFrameworkCore;
using ServiceManagementApi.Models;

namespace ServiceManagementApi.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // ---------- DbSets ----------
    public DbSet<AppUser> Users { get; set; } = null!;
    public DbSet<ServiceCategory> ServiceCategories { get; set; } = null!;
    public DbSet<Service> Services { get; set; } = null!;
    public DbSet<ServiceRequest> ServiceRequests { get; set; } = null!;
    public DbSet<Technician> Technicians { get; set; } = null!;
    public DbSet<Invoice> Invoices { get; set; } = null!;
    public DbSet<TechnicianAssignment> TechnicianAssignments { get; set; } = null!;


    // ---------- Fluent API ----------
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasKey(e => e.UserId);
            entity.Property(e => e.Email).IsRequired();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        modelBuilder.Entity<ServiceCategory>()
            .HasKey(c => c.ServiceCategoryId);

        modelBuilder.Entity<Service>()
            .HasKey(s => s.ServiceId);

        modelBuilder.Entity<ServiceRequest>()
            .HasKey(r => r.ServiceRequestId);

        modelBuilder.Entity<Technician>()
            .HasKey(t => t.TechnicianId);

        modelBuilder.Entity<Technician>()
            .HasOne(t => t.User)
            .WithMany()
            .HasForeignKey(t => t.UserId);

        modelBuilder.Entity<Invoice>()
            .HasKey(i => i.InvoiceId);

        modelBuilder.Entity<TechnicianAssignment>()
            .HasOne(ta => ta.ServiceRequest)
            .WithMany()
            .HasForeignKey(ta => ta.ServiceRequestId);

        modelBuilder.Entity<TechnicianAssignment>()
            .HasOne(ta => ta.Technician)
            .WithMany()
            .HasForeignKey(ta => ta.TechnicianId);
    }
}
