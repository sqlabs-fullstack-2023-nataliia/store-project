using System.ComponentModel.DataAnnotations;

namespace Order.Host.Models;

public class OrderItemModel
{
    [Required(ErrorMessage = "Item id is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Item id must be greater than 0")]
    public int ItemId { get; set; }
    
    [Required(ErrorMessage = "Quantity is required")]
    [Range(0, 35, ErrorMessage = "Quantity must be between 1 and 35")] 
    public int Quantity { get; set; }
    
    [Required(ErrorMessage = "Brand id is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Brand id must be greater than 0")]
    public int BrandId { get; set; }
    
    [Required(ErrorMessage = "Price is required")]
    [Range(1, 1500, ErrorMessage = "Price must be between 1.00 and 1500.00 $")] 
    public decimal Price { get; set; }
    
    [Required(ErrorMessage = "Size is required")]
    [StringLength(40, ErrorMessage = "String length should be less than 40")]
    public string? Size { get; set; }
    
    [Required(ErrorMessage = "Name is required")]
    [StringLength(35, ErrorMessage = "String length should be less than 35")]
    public string? Name { get; set; }
    
    [Required(ErrorMessage = "Image is required")]
    [StringLength(40, ErrorMessage = "String length should be less than 40")]
    public string? Image { get; set; }
    
    public override string ToString()
    {
        return $"OrderItem: id: {ItemId}, quantity: {Quantity}, " +
               $"brandId: {BrandId}, price: {Price}, size: {Size}, name: {Name}, " +
               $"image: {Image}";
    }
    
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        
        OrderItemModel other = (OrderItemModel)obj;
        return Name == other.Name 
               && BrandId == other.BrandId 
               && Price == other.Price 
               && ItemId == other.ItemId
               && Image == other.Image
               && Quantity == other.Quantity 
               && Size == other.Size;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    } 
}