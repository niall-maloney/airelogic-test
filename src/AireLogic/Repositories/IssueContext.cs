using AireLogic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace AireLogic.Repositories
{
    public class IssueContext : DbContext
    {
        public DbSet<Issue> Issues { get; set; }

        public IssueContext(DbContextOptions<IssueContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Issue>(ConfigureIssue);
        }

        private void ConfigureIssue(EntityTypeBuilder<Issue> builder)
        {
            builder.ToTable("issues");

            builder.Property(i => i.Uuid)
                   .HasColumnName("uuid")
                   .IsRequired();

            builder.Property(i => i.ShortDescription)
                   .HasColumnName("short_description")
                   .HasMaxLength(1000)
                   .IsRequired();

            builder.Property(i => i.LongDescription)
                   .HasColumnName("long_description")
                   .HasMaxLength(1000)
                   .IsRequired();

            builder.Property(i => i.Assignee)
                   .HasColumnName("assignee")
                   .IsRequired();

            builder.Property(i => i.DateTimeOpened)
                    .HasColumnName("opened")
                    .HasColumnType("DateTime")
                    .IsRequired();

            builder.Property(i => i.DateTimeClosed)
                   .HasColumnName("closed")
                   .HasColumnType("DateTime2");

            builder.Property(i => i.Status)
                   .HasColumnName("status")
                   .HasMaxLength(50);

            builder.Property(i => i.SelfLink)
                   .HasColumnName("self_link")
                   .HasMaxLength(50);
        }

        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     optionsBuilder.UseSqlServer(@"Server=localhost:1433;Database=airelogic;Trusted_Connection=True;");
        // }
    }
}