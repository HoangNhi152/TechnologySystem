using System.ComponentModel.DataAnnotations;

namespace TechnologySystem.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [StringLength(255)]
        [Display(Name = "CategoryName")]
        public string CategoryName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }
    }
}