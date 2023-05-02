using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace EFCoreCodeFirst.Entities
{
    public class Student
    {
        //Öğrenci Id PK
        public string Id { get; private set; }
        public string Name { get; set; }
        //[StringLength(20)] -- Bu versiyonda önerilmiyor
        public string SurName { get; set; }
        public string Number { get; set; }

        //Öğrencinin Bölümü FK
        public string DepartmentId { get; set; }
        public string LessonsId { get; set; }

        public DateTime? CreatedAt { get; set; }

        //Navigation property Ögrencinin Bölüm Bilgisi
        public StudentDepartment Department { get; set; }
        public List<Lesson> Lessons { get; set; }
        public Student() 
        {
            //Random oluşturuyor.32 numaralık string oluşturuyor.
            Id = Guid.NewGuid().ToString();
        }    


    }
}
