namespace Catalog.Host.DbContextData.Entities;

public class ItemCategory
{
    public int Id { get; set; }
    public string? Category { get; set; }
    
    public override string ToString()
    {
        return $"ItemCategory: id: {Id}, category: {Category}";
    }
    
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        ItemCategory other = (ItemCategory)obj;
        return Id == other.Id && Category == other.Category;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}