namespace Order.Host.DbContextData.Entities;

public class CatalogOrder
{
    public int Id { get; set; }
    public string? Date { get; set; }
    public int TotalQuantity { get; set; }
    public decimal TotalPrice { get; set; }
    public string? UserId { get; set; }
    
    public override string ToString()
    {
        return $"CatalogOrder: id: {Id}, date: {Date}, totalQuantity: {TotalQuantity}, " +
               $"totalPrice: {TotalPrice}, userId: {UserId}";
    }
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        CatalogOrder other = (CatalogOrder)obj;
        return Id == other.Id && Date == other.Date 
              && TotalQuantity == other.TotalQuantity && TotalPrice == other.TotalPrice 
              && UserId == other.UserId;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    
}