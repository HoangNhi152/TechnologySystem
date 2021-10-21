using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechnologySystem.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [StringLength(255)]

        [Required]
        public string CourseName { get; set; }
        [StringLength(255)]

        [Required]
        public string Description { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}