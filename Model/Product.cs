using System.ComponentModel.DataAnnotations;

namespace dotnet8_introduction.Model
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public float Price { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public Category? category { get; set; }
    }
}
