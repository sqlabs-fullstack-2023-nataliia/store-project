import { Reducer } from "react";
import { CatalogItemModel } from "../models/CatalogItemModel";
import { 
    AUTH_ACTION,
    SET_BASKET_ACTION, 
    SET_BRAND_ACTION, 
    SET_BUSKET_ITEMS_COUNT_ACTION, 
    SET_CATALOG_ACTION, 
    SET_CATALOG_ITEM_ACTION, 
    SET_CATEGORY_ACTION, 
    SET_ITEMS_ACTION, 
    SET_TYPE_ACTION, 
} from "./actions";
import { PayloadAction } from "@reduxjs/toolkit";
import { ItemModel } from "../models/ItemModel";
import { ItemBrandModel } from "../models/ItemBrandModel";
import { ItemTypeModel } from "../models/ItemTypeModel";
import { ItemCategoryModel } from "../models/ItemCategoryModel";
import { BasketItemModel } from "../models/BasketItemModel";
import { User } from "oidc-client";


const emptyCatalogItem: CatalogItemModel = {
    id: 0,
    name: '',
    itemBrand: '',
    itemTypeId: 0,
    price: 0,
    image: '',
    itemCategoryId: 0,
    description: ''
}

export const itemsCountReducer: Reducer<number, PayloadAction<number>> = 
(count = 0, action): number => {
    return action.type === SET_BUSKET_ITEMS_COUNT_ACTION ? action.payload : count
}

export const catalogReducer:Reducer<CatalogItemModel[], PayloadAction<CatalogItemModel[]>> =
 (catalog = [], action):CatalogItemModel[] => {
     return action.type === SET_CATALOG_ACTION ? action.payload : catalog;
}

export const catalogItemReducer: Reducer<CatalogItemModel, PayloadAction<CatalogItemModel>> = 
(catalogItem = emptyCatalogItem, action): CatalogItemModel => {
    return action.type === SET_CATALOG_ITEM_ACTION ? action.payload : catalogItem
}

export const itemsRedsucer: Reducer<ItemModel[], PayloadAction<ItemModel[]>> = 
(items = [], action): ItemModel[] => {
    return action.type === SET_ITEMS_ACTION ? action.payload : items;
}

export const brandReducer:Reducer<ItemBrandModel[], PayloadAction<ItemBrandModel[]>> =
 (brands = [], action):ItemBrandModel[] => {
     return action.type === SET_BRAND_ACTION ? action.payload : brands;
}

export const typeReducer:Reducer<ItemTypeModel[], PayloadAction<ItemTypeModel[]>> =
 (types = [], action):ItemTypeModel[] => {
     return action.type === SET_TYPE_ACTION ? action.payload : types;
}

export const categoryReducer:Reducer<ItemCategoryModel[], PayloadAction<ItemCategoryModel[]>> =
 (categories = [], action):ItemCategoryModel[] => {
     return action.type === SET_CATEGORY_ACTION ? action.payload : categories;
}

export const basketReducer:Reducer<BasketItemModel[], PayloadAction<BasketItemModel[]>> =
 (basketItems = [], action):BasketItemModel[] => {
     return action.type === SET_BASKET_ACTION && action.payload.length > 0 ? action.payload : basketItems;
}

export const userDataReducer: Reducer<User | null, PayloadAction<any>> =
(userData = null, action): User | null => {
    return action.type === AUTH_ACTION ? action.payload : userData;
}
