﻿using System;
using CoderDeckAzureFunction;
using CoderDeckAzureFunction.Models;
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

        public virtual DbSet<AppointmentNotes> AppointmentNotes { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=tcp:kmazurelearningsqlserver.database.windows.net,1433;Initial Catalog=CoderDeckPoc;Persist Security Info=False;User ID=kmazurelearning;Password=AzureTemp@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
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
                entity.Property(e => e.UserId);
            });
            modelBuilder.Entity<AppointmentNotes>(entity =>
            {
               
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            });

        }
    }
}
