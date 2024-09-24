import { PayloadAction } from "@reduxjs/toolkit";
import { CatalogItemModel } from "../models/CatalogItemModel";
import { ItemModel } from "../models/ItemModel";
import { ItemBrandModel } from "../models/ItemBrandModel";
import { ItemTypeModel } from "../models/ItemTypeModel";
import { ItemCategoryModel } from "../models/ItemCategoryModel";
import { BasketItemModel } from "../models/BasketItemModel";
import {Profile} from "../models/Profile.ts";

export const SET_CATALOG_ACTION = "/catalog/set";
export const SET_ITEMS_ACTION = "/item/set";
export const SET_BRAND_ACTION = "/brand/set";
export const SET_TYPE_ACTION = "/type/set";
export const SET_CATEGORY_ACTION = "/category/set";
export const SET_BASKET_ACTION = "/basket";
export const SET_CATALOG_ITEM_ACTION = "catalogItem/set";
export const SET_BUSKET_ITEMS_COUNT_ACTION = "items/count";
export const AUTH_ACTION = "auth";

export function setItemsCount(count: number): PayloadAction<number> {
    return {payload: count, type: SET_BUSKET_ITEMS_COUNT_ACTION}
}

export function setCatalog(catalog: CatalogItemModel[]): PayloadAction<CatalogItemModel[]> {
    return {payload: catalog, type: SET_CATALOG_ACTION}
}

export function setItems(items: ItemModel[]): PayloadAction<ItemModel[]> {
    return {payload: items, type: SET_ITEMS_ACTION}
}

export function setBrands(brands: ItemBrandModel[]): PayloadAction<ItemBrandModel[]> {
    return {payload: brands, type: SET_BRAND_ACTION}
}

export function setTypes(types: ItemTypeModel[]): PayloadAction<ItemTypeModel[]> {
    return {payload: types, type: SET_TYPE_ACTION}
}

export function setCategories(categories: ItemCategoryModel[]): PayloadAction<ItemCategoryModel[]> {
    return {payload: categories, type: SET_CATEGORY_ACTION}
}

export function setCatalogItem(catalogItem: CatalogItemModel): PayloadAction<CatalogItemModel> {
    return {payload: catalogItem, type: SET_CATALOG_ITEM_ACTION}
}

export function setUserData(userData: Profile | null): PayloadAction<Profile | null> {
    return {payload: userData, type: AUTH_ACTION}
}

export function setBasket(basketItems: BasketItemModel[]): PayloadAction<BasketItemModel[]> {
    return {payload: basketItems, type: SET_BASKET_ACTION}
}


