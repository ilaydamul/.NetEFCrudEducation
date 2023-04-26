using DotNetEFCrud.Entitites;

namespace DotNetEFCrud.Models
{
    public class CategoryIndexViewModel
    {
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Supplier> Suppliers { get; set; } = new List<Supplier>();

    }
}
