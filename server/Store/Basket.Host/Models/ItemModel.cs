using System.ComponentModel.DataAnnotations;

namespace Basket.Host.Models;

public class ItemModel
{
    [Required(ErrorMessage = "Item id is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Item id must be greater than 0")]
    public int Id { get; set; }
    public CatalogItem? CatalogItem { get; set; }
    
    [Required(ErrorMessage = "Size is required")]
    [StringLength(40, ErrorMessage = "String length should be less than 40")]
    public string? Size { get; set; }
    
    [Required(ErrorMessage = "Quantity is required")]
    [Range(0, 2000, ErrorMessage = "Quantity must be between 1 and 2000")] 
    public int Quantity { get; set; }
    
    public override string ToString()
    {
        return $"ItemNodel: id: {Id}, catalogItem: {CatalogItem.ToString()}, " +
               $"size: {Size}, quantity: {Quantity}";
    }
    
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        ItemModel other = (ItemModel)obj;
        return Id == other.Id
               && Quantity == other.Quantity && Size == other.Size;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}