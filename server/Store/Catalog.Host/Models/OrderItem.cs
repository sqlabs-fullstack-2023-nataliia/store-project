using System.ComponentModel.DataAnnotations;

namespace Catalog.Host.Models;

public class OrderItem
{
    [Required(ErrorMessage = "Item id is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Item id must be greater than 0")]
    public int ItemId { get; set; }
    
    [Required(ErrorMessage = "Quantity is required")]
    [Range(1, 35, ErrorMessage = "Quantity must be in range [1 - 35]")]
    public int Quantity { get; set; }
    
    public override string ToString()
    {
        return $"OrderItem: itemId: {ItemId}, quantity: {Quantity}";
    }
    
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        
        OrderItem other = (OrderItem)obj;
        return ItemId == other.ItemId
               && Quantity == other.Quantity;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}