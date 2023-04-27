using EFCoreCodeFirst.Entities.Configurations;
//using EFCoreCodeFirst.Entities.Views;
using Microsoft.EntityFrameworkCore;

namespace EFCoreCodeFirst.Entities
{
    //Veri tabanı bağlantılarını buradan yönetiyoruz.
    public class AppDbContext:DbContext
    {
        //DbContextOptions<AppDbContext> options Bu kod ile useMysql, useSqlServer gibi farklı opsiyonları constructordan gönderebilme imkanımız oluyor.
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) 
        {
            
        }

        public DbSet<Student> Students { get; set; }// Tablo isimlerinin sonuna s takısı koyarak entity ile karışmasını engelledik. DbSet EF'den gelen özel bir property, bu property ile veri tabanı ile tablolar arasında bir geçiş sağlanır.

        public DbSet<StudentDepartment> StudentDepartments { get; set; }  
        //public DbSet<StudentDepartmentView> StudentDepartmentViews { get; set; }

        //Model oluşurken konfigurasyon işlemlerinin model db'de oluşmadan önce dbye tanıtıldığı yer.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new StudentConfiguration());
            modelBuilder.ApplyConfiguration(new StudentDepartmentConfiguration());

            //modelBuilder.Entity<StudentDepartmentView>.HasNoKey();
            //modelBuilder.ApplyConfiguration(new StudentDepartmentView()).HasNoKey();

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            //Kayıt edilmeden önce ara işlem yapabiliriz.
            foreach(var item in this.ChangeTracker.Entries())
            {
                if(item.State == EntityState.Added)
                {
                    //Add yapmadan önceki kısım
                    if(item.Entity is Student)
                    {
                        var entity = item.Entity as Student;
                        entity.CreatedAt = DateTime.Now; //createdAtleri otomatik ekledik.
                    }
                }
            }

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //Kayıt edilmeden önce ara işlem yapabiliriz.
            foreach (var item in this.ChangeTracker.Entries())
            {
                if (item.State == EntityState.Added)
                {
                    //Add yapmadan önceki kısım
                    if (item.Entity is Student)
                    {
                        var entity = item.Entity as Student;
                        entity.CreatedAt = DateTime.Now; //createdAtleri otomatik ekledik.
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        


    }
}
