import { BasketItemModel } from "../models/BasketItemModel";


export default interface BasketService {

    getBasket(): Promise<BasketItemModel[]>
    addToBasket(id: number): Promise<void>;
    removeFromBasket(id: number): Promise<void>;
    checkoutBasket(): Promise<number>;
}