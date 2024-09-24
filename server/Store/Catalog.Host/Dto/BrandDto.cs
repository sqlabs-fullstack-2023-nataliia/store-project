using System.ComponentModel.DataAnnotations;

namespace Catalog.Host.Dto;

public class BrandDto
{
    [Required(ErrorMessage = "Brand is required")]
    [StringLength(25, ErrorMessage = "String length should be less than 25")]
    public string? Brand { get; set; }
    
    public override string ToString()
    {
        return $"BrandDto: brand: {Brand}";
    }
    
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        BrandDto other = (BrandDto)obj;
        return Brand == other.Brand;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}