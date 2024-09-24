namespace Catalog.Host.DbContextData.Entities;

public class ItemType
{
    public int Id { get; set; }
    public string? Type { get; set; }
    
    public override string ToString()
    {
        return $"ItemType: id: {Id}, type: {Type}";
    }
    
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        ItemType other = (ItemType)obj;
        return Id == other.Id && Type == other.Type;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}