using System.ComponentModel.DataAnnotations;

namespace Catalog.Host.Dto;

public class CategoryDto
{
    [Required(ErrorMessage = "Category is required")]
    [StringLength(25, ErrorMessage = "String length should be less than 25")]
    public string? Category { get; set; }
    
    public override string ToString()
    {
        return $"CategoryDto:, category: {Category}";
    }
    
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        CategoryDto other = (CategoryDto)obj;
        return Category == other.Category;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}