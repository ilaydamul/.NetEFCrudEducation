namespace EFCoreCodeFirst.Entities
{
    public class StudentDepartment
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string? Code { get; private set; }


        //Name alanı constructordan girilmesi zorunlu bırakıldı.
        public StudentDepartment(string name)
        {
            Id=Guid.NewGuid().ToString();
            Name=name;
        }

        //Bölümün Öğrencileri 
        public List<Student> Students { get; set; }
    }
}
