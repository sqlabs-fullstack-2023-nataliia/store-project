using System.ComponentModel.DataAnnotations;

namespace Basket.Host.Models;

public class Item
{
    [Required(ErrorMessage = "Item id is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Item id must be greater than 0")]
    public int ItemId { get; set; }
    
    [Required(ErrorMessage = "Quantity is required")]
    [Range(0, 35, ErrorMessage = "Quantity must be between 1 and 35")]
    public int Quantity { get; set; }
    
    public override string ToString()
    {
        return $"Item: itemId: {ItemId}, quantity: {Quantity}";
    }
    
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        Item other = (Item)obj;
        return ItemId == other.ItemId
               && Quantity == other.Quantity;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

