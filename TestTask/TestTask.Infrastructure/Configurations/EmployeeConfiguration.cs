using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestTask.Domain.Entities;

namespace TestTask.Infrastructure.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            // Map to the Employees table
            builder.ToTable("Employees");

            // Primary key configuration. Configure automatic GUID generation
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            // Configure PayrollNumber: required and with a maximum length of 10 characters
            builder.Property(e => e.PayrollNumber)
                    .IsRequired()
                    .HasMaxLength(10);

            // Configure Forename: required and with a maximum length of 20 characters
            builder.Property(e => e.Forename)
                   .IsRequired()
                   .HasMaxLength(20);

            // Configure Surname: required and with a maximum length of 20 characters
            builder.Property(e => e.Surname)
                   .IsRequired()
                   .HasMaxLength(20);

            // Configure DateOfBirth: required and stored as date
            builder.Property(e => e.DateOfBirth)
                   .IsRequired()
                   .HasColumnType("date");

            // Configure PhoneNumber: optional with a maximum length of 15 characters
            builder.Property(e => e.PhoneNumber)
                   .HasMaxLength(15);

            // Configure MobilePhoneNumber: optional with a maximum length of 15 characters
            builder.Property(e => e.MobilePhoneNumber)
                   .HasMaxLength(15);

            // Configure StreetAddress: required and with a maximum length of 150 characters
            builder.Property(e => e.StreetAddress)
                   .IsRequired()
                   .HasMaxLength(150);

            // Configure Settlement: required and with a maximum length of 20 characters
            builder.Property(e => e.Settlement)
                   .IsRequired()
                   .HasMaxLength(20);

            // Configure Postcode: required and with a maximum length of 10 characters
            builder.Property(e => e.Postcode)
                   .IsRequired()
                   .HasMaxLength(10);

            // Configure Email: required and with a maximum length of 50 characters
            builder.Property(e => e.Email)
                   .IsRequired()
                   .HasMaxLength(50);

            // Configure StartDate: required and stored as date
            builder.Property(e => e.StartDate)
                   .IsRequired()
                   .HasColumnType("date");




        }
    }
}
