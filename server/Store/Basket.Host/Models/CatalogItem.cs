using System.ComponentModel.DataAnnotations;

namespace Basket.Host.Models;

public class CatalogItem
{
    [Required(ErrorMessage = "Catalog item id is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Catalog item id must be greater than 0")]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Name is required")]
    [StringLength(35, ErrorMessage = "String length should be less than 35")]
    public string? Name { get; set; }
    
    [Required(ErrorMessage = "Brand id is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Brand id must be greater than 0")]
    public int ItemBrandId { get; set; }
    
    [Required(ErrorMessage = "Price is required")]
    [Range(1, 1500, ErrorMessage = "Price must be between 1.00 and 1500.00 $")] 
    public decimal Price { get; set; }
    
    [Required(ErrorMessage = "Image is required")]
    [StringLength(40, ErrorMessage = "String length should be less than 40")]
    public string? Image { get; set; }

    public override string ToString()
    {
        return $"CatalogItem: id: {Id}, name: {Name}, " +
               $"itemBrandId: {ItemBrandId}, price: {Price}, image: {Image}";
    }
    
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        CatalogItem other = (CatalogItem)obj;
        return Name == other.Name
               && ItemBrandId == other.ItemBrandId
               && Id == other.Id
               && Price == other.Price;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}