using System.ComponentModel.DataAnnotations;

namespace Catalog.Host.Dto;

public class ItemDto
{
    [Required(ErrorMessage = "Catalog item id is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Catalog item id must be greater than 0")]
    public int CatalogItemId { get; set; }
    
    [Required(ErrorMessage = "Quantity is required")]
    [Range(0, 2000, ErrorMessage = "Quantity must be between 1 and 2000")] 
    public int Quantity { get; set; }
    
    [Required(ErrorMessage = "Size is required")]
    [StringLength(40, ErrorMessage = "String length should be less than 40")]
    public string? Size { get; set; }
    
    public override string ToString()
    {
        return $"ItemDto: catalogItemId: {CatalogItemId}, " +
               $"quantity: {Quantity}, size: {Size}";
    }
    
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        ItemDto other = (ItemDto)obj;
        return CatalogItemId == other.CatalogItemId
                              && Quantity == other.Quantity && Size == other.Size;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}