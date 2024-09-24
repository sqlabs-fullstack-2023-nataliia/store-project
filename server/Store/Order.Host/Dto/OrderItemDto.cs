using System.ComponentModel.DataAnnotations;

namespace Order.Host.Dto;

public class OrderItemDto
{
    [Required(ErrorMessage = "Order id is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Order id must be greater than 0")]
    public int OrderId { get; set; }
    
    [Required(ErrorMessage = "Item id is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Item id must be greater than 0")]
    public int ItemId { get; set; }
    
    [Required(ErrorMessage = "Sub price is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Sub price should be grater than 0")]
    public decimal SubPrice { get; set; }
    
    [Required(ErrorMessage = "Quantity is required")]
    [Range(0, 35, ErrorMessage = "Quantity should be in range [1 - 35]")] 
    public int Quantity { get; set; }
    
    public override string ToString()
    {
        return $"OrderItemDto: orderId: {OrderId}, itemId: {ItemId}, " +
               $"subPrice: {SubPrice}, quantity: {Quantity}";
    }
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        OrderItemDto other = (OrderItemDto)obj;
        return OrderId == other.OrderId 
                              && ItemId == other.ItemId && SubPrice == other.SubPrice 
                              && Quantity == other.Quantity;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}