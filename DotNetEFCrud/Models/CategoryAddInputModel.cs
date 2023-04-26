using System.ComponentModel.DataAnnotations;

namespace DotNetEFCrud.Models
{
    public class CategoryAddInputModel
    {
        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
