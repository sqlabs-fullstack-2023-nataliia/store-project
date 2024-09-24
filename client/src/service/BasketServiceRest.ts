import axios from "axios";
import { BasketItemModel } from "../models/BasketItemModel";
import {CLIENT_DATA_ITEM} from "./AuthServiceClient.ts";
import {emptyClientData, Profile} from "../models/Profile.ts";
import BasketService from "./BasketService.ts";

const DEFAULT_QUANTITY = 1;

export default class BasketServiceRest implements BasketService {

    private _baseUrl: string;

    constructor(baseUrl: string){
        this._baseUrl = baseUrl
    }

    async getBasket(): Promise<BasketItemModel[]> {
        const clientDataString = localStorage.getItem(CLIENT_DATA_ITEM);
        const data: Profile = clientDataString ? JSON.parse(clientDataString) : emptyClientData;
        return await axios.get(`${this._baseUrl}/items`, {
            headers: {
              Authorization: `Bearer ${data.access_token}`,
            },
          })
        .then(result => {
          return result.data
        })
        .catch(err => {
          console.log(err.message);
          return []
        })
    }

    async addToBasket(id: number): Promise<void> {
        const clientDataString = localStorage.getItem(CLIENT_DATA_ITEM);
        const data: Profile = clientDataString ? JSON.parse(clientDataString) : emptyClientData;

        await axios.post(`${this._baseUrl}/item`, {
            itemId: id,
            quantity: DEFAULT_QUANTITY
        }, {
          headers: {
            Authorization: `Bearer ${data.access_token}`,
        },
        })
        .then()
        .catch(err => {
            console.log(err.message)
        })
    }

    async removeFromBasket(id: number): Promise<void> {
        const clientDataString = localStorage.getItem(CLIENT_DATA_ITEM);
        const data: Profile = clientDataString ? JSON.parse(clientDataString) : emptyClientData;
        await axios.delete(`${this._baseUrl}/item/${id}/${DEFAULT_QUANTITY}`, {
            headers: {
              Authorization: `Bearer ${data.access_token}`,
            },
          })
        .then()
        .catch(err => {
            console.log(err.message)
        })
    }
    async checkoutBasket(): Promise<number> {
        const clientDataString = localStorage.getItem(CLIENT_DATA_ITEM);
        const data: Profile = clientDataString ? JSON.parse(clientDataString) : emptyClientData;
        return await axios.post(`${this._baseUrl}/items/checkout`, {}, {
            headers: {
              Authorization: `Bearer ${data.access_token}`,
            },
          })
        .then(result => {
            return result.data;
        })
        .catch(err => {
          console.log(err.message)
        })
    }
    
}