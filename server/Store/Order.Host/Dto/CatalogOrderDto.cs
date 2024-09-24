using System.ComponentModel.DataAnnotations;

namespace Order.Host.Dto;

public class CatalogOrderDto
{
    [Required(ErrorMessage = "Date is required")]
    [RegularExpression(@"^(0?[1-9]|1[0-2])/(0?[1-9]|1\d|2\d|3[01])/(19|20)\d{2}$")]
    public string? Date { get; set; }
    
    [Required(ErrorMessage = "Total quantity is required")]
    [Range(1, 35, ErrorMessage = "Total quantity should be in range [1 - 35]")]
    public int TotalQuantity { get; set; }
    
    [Required(ErrorMessage = "Total price is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Total price should be grater than 0")]
    public decimal TotalPrice { get; set; }
    
    [Required(ErrorMessage = "User id is required")]
    [StringLength(100, ErrorMessage = "String length should be less than 100")]
    public string? UserId { get; set; }
    
    public override string ToString()
    {
        return $"CatalogOrderDto: date: {Date}, totalQuantity: {TotalQuantity}, " +
               $"totalPrice: {TotalPrice}, userId: {UserId}";
    }
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        CatalogOrderDto other = (CatalogOrderDto)obj;
        return Date == other.Date 
               && TotalQuantity == other.TotalQuantity && TotalPrice == other.TotalPrice 
               && UserId == other.UserId;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}