import axios from "axios";
import { CatalogItemModel } from "../models/CatalogItemModel";
import { Filter } from "../models/Filter";
import { ItemBrandModel } from "../models/ItemBrandModel";
import { ItemCategoryModel } from "../models/ItemCategoryModel";
import { ItemModel } from "../models/ItemModel";
import { ItemTypeModel } from "../models/ItemTypeModel";
import CatalogService from "./CatalogService";


export default class CatalogServiceRest implements CatalogService {

    private _baseUrl: string;

    constructor(baseUrl: string){
        this._baseUrl = baseUrl
    }

    async getCatalog(filter: Filter): Promise<CatalogItemModel[]> {
        return await axios.get(`${this._baseUrl}/catalog-items?category=${filter.category}&type=${filter.type}&brand=${filter.brand}`)
            .then(result => {
                return result.data;
            })
            .catch(err => {
                console.log(err.message);
                return [];
            });
    }
    async getItems(id: number): Promise<ItemModel[]> {
        return await axios.get(`${this._baseUrl}/items/stock?catalogItemId=${id}`)
        .then(result => {
            return result.data;
        })
        .catch(err => {
            console.error(err.message);
            return [];
        })
    }

    async getCatalogItem(id: number): Promise<CatalogItemModel | null> {
      return await axios.get(`${this._baseUrl}/catalog-items/${id}`)
        .then(result => {
            return result.data;
        })
        .catch(err => {
            console.error(err.message);
            return null;
        })
    }

    async getBrands(): Promise<ItemBrandModel[]> {
        return await axios.get(`${this._baseUrl}/brands`)
        .then(result => {
          return result.data
        })
        .catch(err => {
          console.log(err.message)
          return []
        })
    }
    async getTypes(): Promise<ItemTypeModel[]> {
        return await axios.get(`${this._baseUrl}/types`)
        .then(result => {
          return result.data
        })
        .catch(err => {
          console.log(err.message)
          return []
        })
    }
    async getCategories(): Promise<ItemCategoryModel[]> {
        return await axios.get(`${this._baseUrl}/categories`)
        .then(result => {
          return result.data;
        })
        .catch(err => {
          console.log(err.message)
          return [];
        })
    }
    
}