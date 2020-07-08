
namespace CustomerApi.Infrastructure.Mappings
{
    using CustomerApi.Domains.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CustomerMap : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            // Table & Column Mappings
            builder.ToTable("Customer", "dbo");
            builder.Property(t => t.FirstName).HasColumnName("FirstName").IsRequired().HasMaxLength(50);
            builder.Property(t => t.LastName).HasColumnName("LastName").IsRequired().HasMaxLength(50);
            builder.Property(t => t.Birthday).HasColumnName("Birthday");
            builder.Property(t => t.Age).HasColumnName("Age");

            //builder.HasOne(x => x.DomainManager)
            //    .WithMany().HasForeignKey(x => x.DomainManagerId);

            //builder.HasQueryFilter(x => !x.IsDisabled);
        }
    }
}
