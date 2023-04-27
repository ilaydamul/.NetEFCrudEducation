using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreCodeFirst.Entities.Configurations
{
    //Yönteme Fuent API yöntemi deniliyor.
    //Bu dosyada Student Program nesnesinin POCO Class, veri tabanında aşağıdaki özelleştirmeler ile tutulması için ayarlamalar yaptığımız dosya StudentConfiguration dosyasıdır.
    //Veri tabanında herhangi bir anlamı olmayan tiplere POCO Class denir. -POCO(Plain Old CLR Object)
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(20).IsRequired();//Boş geçilemez ve NVARCHAR 20 Yap
            builder.Property(x => x.SurName).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Number).HasMaxLength(20);

            builder.HasIndex(x=>x.Number).IsUnique();//Öğrenci numarası eşsiz olmalı, aynı numara başka öğrenciye verilemez. Unique Index

            //Açık bir şekilde iki nesne arasında ilişki verme yöntemi.
            builder.HasOne(x => x.Department).WithMany(x => x.Students).HasForeignKey(x => x.DepartmentId);

        }
    }
}
