import { combineReducers, configureStore } from "@reduxjs/toolkit"

import { CatalogItemModel } from "../models/CatalogItemModel"
import { ItemBrandModel } from "../models/ItemBrandModel"
import { ItemCategoryModel } from "../models/ItemCategoryModel"
import { ItemModel } from "../models/ItemModel"
import { ItemTypeModel } from "../models/ItemTypeModel"
import { 
    basketReducer, 
    brandReducer, 
    catalogItemReducer, 
    catalogReducer, 
    categoryReducer, 
    itemsCountReducer, 
    itemsRedsucer, 
    typeReducer, 
    userDataReducer} from "./reducers"
import { BasketItemModel } from "../models/BasketItemModel"
import {Profile} from "../models/Profile.ts";


export type StateType = {
    catalog: CatalogItemModel[],
    items: ItemModel[],
    brands: ItemBrandModel[],
    types: ItemTypeModel[],
    categories: ItemCategoryModel[],
    basketItems: BasketItemModel[],
    catalogItem: CatalogItemModel,
    count: number,
    userData: Profile | null
}

const reducer = combineReducers<StateType> ({
    catalog: catalogReducer as any,
    items: itemsRedsucer as any,
    brands: brandReducer as any,
    types: typeReducer as any,
    categories: categoryReducer as any,
    basketItems: basketReducer as any,
    catalogItem: catalogItemReducer as any,
    count: itemsCountReducer as any,
    userData: userDataReducer as any
})

export const store = configureStore({reducer,
    middleware: (getMiddleware) => getMiddleware({
        serializableCheck: false
})})


