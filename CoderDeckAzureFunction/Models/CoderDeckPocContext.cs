using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CoderDeckServerlessPOC.Models
{
    public partial class CoderDeckPocContext : DbContext
    {
        public CoderDeckPocContext()
        {
        }

        public CoderDeckPocContext(DbContextOptions<CoderDeckPocContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Appointment> Appointment { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=tcp:coderdeckserver.database.windows.net,1433;User ID=coderdeck@gmail.com@coderdeckserver;password=Jdvc@2016;Database=CoderDeckPoc;Trusted_Connection=False;Encrypt=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.Property(e => e.Appointment1)
                    .HasColumnName("Appointment")
                    .HasMaxLength(2000);

                entity.Property(e => e.AppointmentDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(50);
            });
        }
    }
}
