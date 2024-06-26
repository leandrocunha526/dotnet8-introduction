using System.ComponentModel.DataAnnotations;
using dotnet8_introduction.Entities;

namespace dotnet8_introduction.Model
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public float Price { get; set; }

        [Required]
        public string Description { get; set; }

        public Category Category { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}
