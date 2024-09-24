import { CatalogItemModel } from "../models/CatalogItemModel";
import { Filter } from "../models/Filter";
import { ItemBrandModel } from "../models/ItemBrandModel";
import { ItemCategoryModel } from "../models/ItemCategoryModel";
import { ItemModel } from "../models/ItemModel";
import { ItemTypeModel } from "../models/ItemTypeModel";


export default interface CatalogService {

    getCatalog(filter: Filter): Promise<CatalogItemModel[]>;
    getItems(id: number): Promise<ItemModel[]>;
    getCatalogItem(id: number): Promise<CatalogItemModel | null>;
    getBrands(): Promise<ItemBrandModel[]>;
    getTypes(): Promise<ItemTypeModel[]>;
    getCategories(): Promise<ItemCategoryModel[]>;
}