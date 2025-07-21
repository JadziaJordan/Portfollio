using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FreeLancerJadzia.Models;

public partial class FreelancerDbContext : DbContext
{
    public FreelancerDbContext()
    {
    }

    public FreelancerDbContext(DbContextOptions<FreelancerDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<BookingType> BookingTypes { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Freelancer> Freelancers { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<UserP> UserPs { get; set; }

   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__Booking__73951AED1CDBE655");

            entity.ToTable("Booking");

            entity.HasIndex(e => e.PaymentId, "UQ__Booking__9B556A39BC7029B4").IsUnique();

            entity.Property(e => e.BookingDate).HasColumnType("datetime");
            entity.Property(e => e.BookingStatus).HasMaxLength(50);

            entity.HasOne(d => d.BookingType).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.BookingTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Booking__Booking__34C8D9D1");

            entity.HasOne(d => d.Customer).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Booking__Custome__32E0915F");

            entity.HasOne(d => d.Freelancer).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.FreelancerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Booking__Freelan__33D4B598");
        });

        modelBuilder.Entity<BookingType>(entity =>
        {
            entity.HasKey(e => e.BookingTypeId).HasName("PK__BookingT__649EC4916E38E772");

            entity.ToTable("BookingType");

            entity.Property(e => e.TypeName).HasMaxLength(255);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64D8EF988770");

            entity.ToTable("Customer");

            entity.HasIndex(e => e.UserId, "UQ__Customer__1788CC4DFE52BA0D").IsUnique();

            entity.HasOne(d => d.User).WithOne(p => p.Customer)
                .HasForeignKey<Customer>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Customer__UserId__286302EC");
        });

        modelBuilder.Entity<Freelancer>(entity =>
        {
            entity.HasKey(e => e.FreelancerId).HasName("PK__Freelanc__3D00E36CCF8435E7");

            entity.ToTable("Freelancer");

            entity.HasIndex(e => e.UserId, "UQ__Freelanc__1788CC4D6A6241ED").IsUnique();

            entity.Property(e => e.PortfolioUrl).HasMaxLength(500);
            entity.Property(e => e.Skills).HasMaxLength(500);

            entity.HasOne(d => d.User).WithOne(p => p.Freelancer)
                .HasForeignKey<Freelancer>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Freelance__UserI__2C3393D0");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__9B556A381004E28F");

            entity.ToTable("Payment");

            entity.HasIndex(e => e.BookingId, "UQ__Payment__73951AEC061C5F84").IsUnique();

            entity.HasIndex(e => e.CustomerId, "UQ__Payment__A4AE64D9978F1246").IsUnique();

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentStatus).HasMaxLength(50);

            entity.HasOne(d => d.Booking).WithOne(p => p.Payment)
                .HasForeignKey<Payment>(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payment__Booking__398D8EEE");

            entity.HasOne(d => d.Customer).WithOne(p => p.Payment)
                .HasForeignKey<Payment>(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payment__Custome__3A81B327");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__Reviews__74BC79CEE433A0EA");

            entity.HasIndex(e => e.BookingId, "UQ__Reviews__73951AEC95FB85FE").IsUnique();

            entity.Property(e => e.Comment).HasMaxLength(1000);

            entity.HasOne(d => d.Booking).WithOne(p => p.Review)
                .HasForeignKey<Review>(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reviews__Booking__403A8C7D");

            entity.HasOne(d => d.Customer).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reviews__Custome__412EB0B6");

            entity.HasOne(d => d.Freelancer).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.FreelancerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reviews__Freelan__3F466844");
        });

        modelBuilder.Entity<UserP>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__userP__1788CC4C2DD6A7F3");

            entity.ToTable("userP");

            entity.HasIndex(e => e.Email, "UQ__userP__A9D10534DEE89DC7").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.PasswordHash).HasMaxLength(500);
            entity.Property(e => e.Roles).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
