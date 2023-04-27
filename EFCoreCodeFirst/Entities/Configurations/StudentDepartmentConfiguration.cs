using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreCodeFirst.Entities.Configurations
{
    public class StudentDepartmentConfiguration : IEntityTypeConfiguration<StudentDepartment>
    {
        public void Configure(EntityTypeBuilder<StudentDepartment> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Name).IsUnique();
            builder.Property(x => x.Code).HasMaxLength(15);

            //Burada ilişki tanıma gerek yok. Tek bir yerde yazılması yeterlidir. Burada yapsaydık aşağıdaki gibi yazardık:
            //builder.HasMany(st => st.Students).WithOne(dp=>dp.Department).HasForeignKey(st>st.DepartmentId);
        }
    }
}
