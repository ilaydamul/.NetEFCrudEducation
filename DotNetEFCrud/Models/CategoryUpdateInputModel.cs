using System.ComponentModel.DataAnnotations;

namespace DotNetEFCrud.Models
{
    public class CategoryUpdateInputModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
