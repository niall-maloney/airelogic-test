using AireLogic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AireLogic.Repositories
{
    public class PersonContext : DbContext
    {
        public DbSet<Person> People { get; set; }

        public PersonContext(DbContextOptions<PersonContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Person>(ConfigurePerson);
        }

        private static void ConfigurePerson(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable("people");

            builder.Property(p => p.Uuid)
                   .HasColumnName("uuid")
                   .IsRequired();

            builder.Property(p => p.FirstName)
                   .HasColumnName("first_name")
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(p => p.LastName)
                   .HasColumnName("last_name")
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(p => p.SelfLink)
                   .HasColumnName("self_link")
                   .HasMaxLength(50);
        }
    }
}