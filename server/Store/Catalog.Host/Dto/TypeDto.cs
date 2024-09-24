using System.ComponentModel.DataAnnotations;

namespace Catalog.Host.Dto;

public class TypeDto
{
    [Required(ErrorMessage = "Type is required")]
    [StringLength(25, ErrorMessage = "String length should be less than 25")]
    public string? Type { get; set; }
    
    public override string ToString()
    {
        return $"TypeDto: type: {Type}";
    }
    
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        TypeDto other = (TypeDto)obj;
        return Type == other.Type;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}