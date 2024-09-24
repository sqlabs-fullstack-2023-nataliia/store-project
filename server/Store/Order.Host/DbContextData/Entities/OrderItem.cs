namespace Order.Host.DbContextData.Entities;

public class OrderItem
{
    
    public int Id { get; set; }
    public int OrderId { get; set; }
    public CatalogOrder? Order { get; set; }
    public int ItemId { get; set; }
    public decimal SubPrice { get; set; }
    public int Quantity { get; set; }
    
    public override string ToString()
    {
        return $"OrderItem: id: {Id}, orderId: {OrderId}, itemId: {ItemId}, " +
               $"subPrice: {SubPrice}, quantity: {Quantity}";
    }
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        OrderItem other = (OrderItem)obj;
        return Id == other.Id && OrderId == other.OrderId 
             && ItemId == other.ItemId && SubPrice == other.SubPrice 
             && Quantity == other.Quantity;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}