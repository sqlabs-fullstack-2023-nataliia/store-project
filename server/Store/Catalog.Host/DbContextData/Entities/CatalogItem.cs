namespace Catalog.Host.DbContextData.Entities;

public class CatalogItem
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int ItemBrandId { get; set; }
    public ItemBrand? ItemBrand { get; set; }
    public int ItemTypeId { get; set; }
    public ItemType? ItemType { get; set; }
    public decimal Price { get; set; }
    public string? Image { get; set; }
    public int ItemCategoryId { get; set; }
    public ItemCategory? ItemCategory { get; set; }
    public string? Description { get; set; }
    
    public override string ToString()
    {
        return $"CatalogItem: id: {Id}, name: {Name}, " +
               $"itemBrandId: {ItemBrandId}, itemTypeId: {ItemTypeId}, " +
               $"price: {Price}, image: {Image}, itemCategoryId: {ItemCategoryId}, " +
               $"description: {Description}";
    }
    
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        CatalogItem other = (CatalogItem)obj;
        return Id == other.Id && Name == other.Name 
                              && ItemBrandId == other.ItemBrandId 
                              && ItemTypeId == other.ItemTypeId
                              && Price == other.Price 
                              && ItemCategoryId == other.ItemCategoryId;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}