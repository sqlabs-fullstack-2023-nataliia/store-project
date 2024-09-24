using Catalog.Host.DbContextData.Entities;

namespace Catalog.Host.DbContextData;

public static class DbInitializer
{
    public static async Task Initialize(ApplicationDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        if (!context.ItemBrands.Any())
        {
            await context.ItemBrands.AddRangeAsync(GetPreconfiguredCatalogBrands());

            await context.SaveChangesAsync();
        }
        
        if (!context.ItemTypes.Any())
        {
            await context.ItemTypes.AddRangeAsync(GetPreconfiguredCatalogTypes());

            await context.SaveChangesAsync();
        }
        
        if (!context.ItemCategories.Any())
        {
            await context.ItemCategories.AddRangeAsync(GetPreconfiguredCatalogCategories());

            await context.SaveChangesAsync();
        }
        
        if (!context.CatalogItems.Any())
        {
            await context.CatalogItems.AddRangeAsync(GetPreconfiguredCatalogCatalogItems());

            await context.SaveChangesAsync();
        }
        
        if (!context.Items.Any())
        {
            await context.Items.AddRangeAsync(GetPreconfiguredCatalogStocks());

            await context.SaveChangesAsync();
        }
    }

    private static IEnumerable<ItemBrand> GetPreconfiguredCatalogBrands()
    {
        return new List<ItemBrand>
        {
            new() { Brand = "Nike" },
            new() { Brand = "Adidas" },
            new() { Brand = "Gucci" },
            new() { Brand = "Zara" },
            new() { Brand = "Converse" },
            new() { Brand = "Puma" },
            new() { Brand = "H&M" },
            new() { Brand = "Vans" },
            new() { Brand = "Fendi" },
            new() { Brand = "Balenciaga" },
        };
    }
    
    private static IEnumerable<ItemType> GetPreconfiguredCatalogTypes()
    {
        return new List<ItemType>
        {
            new() { Type = "Shoes" },
            new() { Type = "Bags" },
            new() { Type = "Pants" },
            new() { Type = "Jackets" },
            new() { Type = "T-Shirts" },
            new() { Type = "Dresses" },
            new() { Type = "Hats" },
            new() { Type = "Sunglasses" },
            new() { Type = "Sweaters" },
            new() { Type = "Skirts" },
            new() { Type = "Swimwear" },
        };
    }
    
    private static IEnumerable<ItemCategory> GetPreconfiguredCatalogCategories()
    {
        return new List<ItemCategory>
        {
            new() { Category = "MAN" },
            new() { Category = "WOMAN" },
            new() { Category = "KIDS" },
        };
    }
    
    private static IEnumerable<CatalogItem> GetPreconfiguredCatalogCatalogItems()
    {
        return new List<CatalogItem>
        {
            new()
            {
                Name = "Air Max 270",
                ItemBrandId = 1,
                ItemTypeId = 1,
                Price = 120.99m,
                Image = "1.png",
                ItemCategoryId = 1,
                Description = "Premium running shoes with advanced cushioning technology."
            },
            new()
            {
                Name = "Air Max 270",
                ItemBrandId = 1,
                ItemTypeId = 1,
                Price = 120.99m,
                Image = "1.png",
                ItemCategoryId = 2,
                Description = "Premium running shoes with advanced cushioning technology."
            },
            new()
            {
                Name = "Air Max 270",
                ItemBrandId = 1,
                ItemTypeId = 1,
                Price = 120.99m,
                Image = "1.png",
                ItemCategoryId = 3,
                Description = "Premium running shoes with advanced cushioning technology."
            },
            new()
            {
                Name = "Leather Handbag",
                ItemBrandId = 3,
                ItemTypeId = 2,
                Price = 299.50m,
                Image = "2.png",
                ItemCategoryId = 2,
                Description = "Genuine leather handbag with spacious compartments."
            },
            new()
            {
                Name = "Slim Fit Jeans",
                ItemBrandId = 4,
                ItemTypeId = 9,
                Price = 49.99m,
                Image = "3.png",
                ItemCategoryId = 1,
                Description = "Classic denim jeans with a slim fit for men."
            },
            new()
            {
                Name = "Running Jacket",
                ItemBrandId = 1,
                ItemTypeId = 4,
                Price = 89.99m,
                Image = "4.png",
                ItemCategoryId = 2,
                Description = "Lightweight running jacket with moisture-wicking fabric."
            },
            new()
            {
                Name = "Summer Dress",
                ItemBrandId = 2,
                ItemTypeId = 5,
                Price = 59.99m,
                Image = "5.png",
                ItemCategoryId = 2,
                Description = "Floral print summer dress for women."
            },
            new()
            {
                Name = "Summer Dress",
                ItemBrandId = 4,
                ItemTypeId = 5,
                Price = 59.99m,
                Image = "6.png",
                ItemCategoryId = 2,
                Description = "Print summer dress for women."
            },
            new()
            {
                Name = "Dress",
                ItemBrandId = 3,
                ItemTypeId = 5,
                Price = 59.99m,
                Image = "7.png",
                ItemCategoryId = 2,
                Description = "Summer dress for women."
            },
            new()
            {
                Name = "Classic T-Shirt",
                ItemBrandId = 5,
                ItemTypeId = 6,
                Price = 29.99m,
                Image = "8.png",
                ItemCategoryId = 3,
                Description = "Comfortable and versatile classic T-shirt for everyday wear."
            },
            new()
            {
                Name = "Running Shoes",
                ItemBrandId = 1,
                ItemTypeId = 1,
                Price = 129.99m,
                Image = "9.png",
                ItemCategoryId = 2,
                Description = "High-performance running shoes with responsive cushioning."
            },
            new()
            {
                Name = "Leather Backpack",
                ItemBrandId = 3,
                ItemTypeId = 2,
                Price = 179.50m,
                Image = "10.png",
                ItemCategoryId = 2,
                Description = "Stylish leather backpack with multiple compartments."
            },
            new()
            {
                Name = "Chino Pants",
                ItemBrandId = 4,
                ItemTypeId = 8,
                Price = 69.99m,
                Image = "11.png",
                ItemCategoryId = 1,
                Description = "Classic chino pants with a modern fit for men."
            },
            new()
            {
                Name = "Winter Jacket",
                ItemBrandId = 2,
                ItemTypeId = 4,
                Price = 149.99m,
                Image = "12.png",
                ItemCategoryId = 1,
                Description = "Insulated winter jacket for protection in cold weather."
            },
            new()
            {
                Name = "Cocktail Dress",
                ItemBrandId = 5,
                ItemTypeId = 5,
                Price = 89.99m,
                Image = "13.png",
                ItemCategoryId = 2,
                Description = "Elegant cocktail dress for special occasions."
            },
            new()
            {
                Name = "Graphic T-Shirt",
                ItemBrandId = 1,
                ItemTypeId = 6,
                Price = 24.99m,
                Image = "14.png",
                ItemCategoryId = 3,
                Description = "Stylish graphic T-shirt with unique artwork."
            }
        };
    }
    
    private static IEnumerable<Item> GetPreconfiguredCatalogStocks()
    {
        return new List<Item>
        {
            new Item { CatalogItemId = 1, Quantity = 11, Size = "39" },
            new Item { CatalogItemId = 1, Quantity = 12, Size = "40" },
            new Item { CatalogItemId = 1, Quantity = 5, Size = "41" },
            new Item { CatalogItemId = 1, Quantity = 5, Size = "42" },
            new Item { CatalogItemId = 1, Quantity = 12, Size = "43" },
            new Item { CatalogItemId = 1, Quantity = 0, Size = "44" },
            new Item { CatalogItemId = 2, Quantity = 11, Size = "36" },
            new Item { CatalogItemId = 2, Quantity = 12, Size = "37" },
            new Item { CatalogItemId = 2, Quantity = 0, Size = "38" },
            new Item { CatalogItemId = 2, Quantity = 5, Size = "39" },
            new Item { CatalogItemId = 3, Quantity = 11, Size = "25" },
            new Item { CatalogItemId = 3, Quantity = 12, Size = "26" },
            new Item { CatalogItemId = 3, Quantity = 0, Size = "27" },
            new Item { CatalogItemId = 3, Quantity = 5, Size = "30" },
            
            new Item { CatalogItemId = 4, Quantity = 12, Size = "one size" },
            
            new Item { CatalogItemId = 5, Quantity = 1, Size = "xs" },
            new Item { CatalogItemId = 5, Quantity = 1, Size = "m" },
            new Item { CatalogItemId = 5, Quantity = 1, Size = "l" },
            new Item { CatalogItemId = 5, Quantity = 2, Size = "xl" },
            
            new Item { CatalogItemId = 6, Quantity = 5, Size = "US 7" },
            new Item { CatalogItemId = 6, Quantity = 8, Size = "US 8" },
            new Item { CatalogItemId = 6, Quantity = 3, Size = "US 9" },
            
            new() { CatalogItemId = 7, Quantity = 1, Size = "s" },
            new() { CatalogItemId = 7, Quantity = 3, Size = "m" },
            new() { CatalogItemId = 7, Quantity = 1, Size = "l" },
            new() { CatalogItemId = 7, Quantity = 2, Size = "xl" },
            new() { CatalogItemId = 8, Quantity = 1, Size = "xs" },
            new() { CatalogItemId = 9, Quantity = 1, Size = "m" },
            new() { CatalogItemId = 10, Quantity = 1, Size = "l" },
            new() { CatalogItemId = 11, Quantity = 2, Size = "xl" },
        };
    }
}