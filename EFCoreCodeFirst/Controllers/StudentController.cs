using EFCoreCodeFirst.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreCodeFirst.Controllers
{

    public class StudentController : Controller
    {
        private AppDbContext dbContext;

        public StudentController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("ogrenci-kayit",Name = "studentCreateRoute")]
        public async Task<IActionResult> Index()
        {
            var students= await dbContext.Students.ToListAsync();
            //Hiç kayıt yoksa gir.
            if(students.Count == 0)
            {
                var bolum1 = new StudentDepartment("Bilişim");
                var bolum2 = new StudentDepartment("Elektrik");
                var stList = new List<Student>
            {
                new Student
                {
                    Name="Ali",
                    SurName="Tan",
                    Number="01",
                    DepartmentId=bolum1.Id,
                    Department=bolum1

                },
                 new Student
                {
                    Name="Ahmet",
                    SurName="Günay",
                    Number="02",
                    Department=bolum1,
                      DepartmentId=bolum1.Id

                },
                  new Student
                {
                    Name="Kasım",
                    SurName="Ay",
                    Number="03",
                    Department=bolum2,
                      DepartmentId=bolum2.Id

                },
                   new Student
                {
                    Name="Ayşe",
                    SurName="Tipsi",
                    Number="04",
                    Department=bolum1,
                      DepartmentId=bolum1.Id

                },
                    new Student
                {
                    Name="Fatma",
                    SurName="Alikan",
                    Number="05",
                    Department=bolum2,
                      DepartmentId=bolum2.Id

                }
            };


                //Tek seferde bulk insert
                await this.dbContext.Students.AddRangeAsync(stList.ToArray());
                //var result = dbContext.SaveChangesAsync().GetAwaiter().GetResult();
                var result = await dbContext.SaveChangesAsync();
            }
            
         

            return View();
        }

        public async Task<IActionResult> Queries()
        {
            //Öğrencileri numarasına göre sıralama (OrderBy küçükten büyüğe sıralar.)
            var o1 =await dbContext.Students.OrderBy(x => x.Number).ToListAsync();

            //Öğrencileri numarasına göre tersten sıralama(OrderByDescending büyükten küçüğe sıralar)
            var o2 = await dbContext.Students.OrderByDescending(x => x.Number).ToListAsync();

            //İki farklı alana göre sıralama yapmak için thenby
            //Adları aynı ise soyada bakacak sıralama
            var o3 = await dbContext.Students.OrderBy(x => x.Name).ThenBy(x => x.SurName).ToListAsync();

            //Öğrencileri soyada göre arama(Where)
            var o4=await dbContext.Students.Where(x=>x.SurName=="Tan").ToListAsync();

            //Soyadı ka ile başlayan öğrenciler
            var o5=await dbContext.Students.Where(x=>x.SurName.StartsWith("Ka")).ToListAsync();

            //Endswith
            //Adında e geçen öğrenciler
            var o6=await dbContext.Students.Where(x=>x.Name.Contains("e")).ToListAsync();

            //Bölümü bilişim olan öğrencileri filtreleme
            //Not: Lazy Loading özelliği core versiyonda kalktı.
            var o7 = await dbContext.Students.Include(x => x.Department).Where(x => x.Department.Name == "Bilişim").ToListAsync();

            //createdAt alanı boş geçilen öğrenci kayıtlarını bulma
            var o8= await dbContext.Students.Where(x=>x.CreatedAt == null).ToListAsync();

            //Hangi bölümde kaç öğrenci var
            //var o9 = await dbContext.Students
            //    .Include(x=>x.Department)
            //    .GroupBy(x => x.DepartmentId)
            //    .Select(a => new
            //{
            //    BolumAdi=a.FirstOrDefault()==null ? "" : a.FirstOrDefault().Department.Name,
            //    KisiSayisi=a.Count()
            //}).ToListAsync();

            //LINQ
            //var o10 = await (from s in dbContext.Students join sd in dbContext.StudentDepartments on s.DepartmentId equals sd.Id
            //           group s by s.DepartmentId into g
            //           select new
            //           {
            //               BolumAdi = g.FirstOrDefault() == null ? "" : g.FirstOrDefault().Department.Name,
            //               KisiSayisi = g.Count()
            //           }).ToArrayAsync();

            //RawSql
            var o112 = dbContext.Students.FromSqlRaw("select * from Students");

            //var o11 = dbContext.StudentDepartmentViews.FromSqlRaw("select Count(*) as StudentCount, sd.Name as DepartmentName from Students s inner join StudentDepartments sd on s.DepartmentId = sd.Id group by s.DepartmentId, sd.Name").AsNoTracking().ToList();

            //Öğrencileri bölümleri ile birlikte sorgulama


            //Elektronik bölümü var mı
            var o12 = dbContext.StudentDepartments.Any(x => x.Name == "Elektronik");


            //Kaç tane öğrencim var?
            var o13 = await dbContext.Students.CountAsync();

            //c274c524-d998-4e2b-9dea-fe55fa58103b nolu departmandaki öğrenciler
            var o14 = await dbContext.Students.FindAsync("c274c524-d998-4e2b-9dea-fe55fa58103b"); // Bulamaz ise null, bulursa tek kayıt döndürür.

            //AsNoTracking => EF'nin nesneleri takip etmemesini sağlıyor. Ekleme / güncelleme /sileceksem açarım. Veri göstereceksem ToList/FirstOrDefault(öncesi) sorgularında AsNoTracking yazmamız gerekir!!!
            //Find AsNoTracking çalışır.
            var o15 = await dbContext.Students.AsNoTracking().FirstOrDefaultAsync(x => x.Id == "c274c524-d998-4e2b-9dea-fe55fa58103b");

            //Sayfalama işlemleri için nasıl sorgularız. 1000 kaydı 100erli şekilde göstermek vs istiyorsak; Sayfalama yapacaksak OrderBy mutlaka olmalı-hata verir-
            //2 Kayıt atlat, 3 kayıt al 
            var o16 = await dbContext.Students.Include(x => x.Department).OrderBy(x => x.Name).Skip(2).Take(3).ToListAsync();

            //Fizik dersindeki öprencileri getiren sorgu
            var s1 = (await dbContext.Lessons.Include(x => x.Students).FirstOrDefaultAsync(x => x.Name == "Fizik")).Students.ToList();

            //Adı Kasım olan öğrencinin girdiği dersler
            var s2 = (await dbContext.Students.Include(x => x.Lessons).FirstOrDefaultAsync(x => x.Number == "03")).Lessons.ToList();



            return View();
        }

        [HttpGet("ogrenci-listesi",Name ="ogrenciSayfaRoute")]
        public async Task<IActionResult> SayfaliGetir(int currentPage=1,int limit=2)
        {

            ViewBag.ToplamSayfaSayisi = await dbContext.Students.CountAsync() / limit;

            var o16 = await dbContext.Students.Include(x => x.Department).OrderBy(x => x.Name).Skip((currentPage - 1) * limit).Take(limit).ToListAsync();

            return View(o16);
        }
    }
}
