using Microsoft.EntityFrameworkCore;
using OnlineClinic.Appointments.Models;
using OnlineClinic.Customers.Models;
using OnlineClinic.Doctors.Models;
using OnlineClinic.DoctorServices.Models;
using OnlineClinic.Services.Models;

namespace OnlineClinic.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){ }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<DoctorService> DoctorServices { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customers");
                entity.Property(s => s.Email).IsRequired().HasMaxLength(256);
                entity.Property(s => s.NormalizedEmail).HasMaxLength(256);
                entity.Property(s => s.UserName).IsRequired().HasMaxLength(256);
                entity.Property(s => s.NormalizedUserName).HasMaxLength(256);
                entity.Property(s => s.Name).IsRequired().HasMaxLength(100);
                entity.Property(s => s.PhoneNumber).IsRequired().HasMaxLength(256);

                entity.HasDiscriminator<string>("Discriminator").HasValue("Customer");

            });

            modelBuilder.Entity<DoctorService>()
            .HasOne(a => a.Service)
            .WithMany(a => a.Doctors)
            .HasForeignKey(a => a.ServiceId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DoctorService>()
            .HasOne(a => a.Doctor)
            .WithMany(a => a.Services)
            .HasForeignKey(a => a.DoctorId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Service)
            .WithMany(a => a.Appointments)
            .HasForeignKey(a => a.ServiceId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Customer)
            .WithMany(a => a.Appointments)
            .HasForeignKey(a => a.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Appointment>()
           .HasOne(a => a.Doctor)
           .WithMany(a => a.Appointments)
           .HasForeignKey(a => a.DoctorId)
           .OnDelete(DeleteBehavior.Cascade);

        }


    }
}
