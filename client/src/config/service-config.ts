import AuthServiceClient from "../service/AuthServiceClient";
import BasketServiceRest from "../service/BasketServiceRest";
import CatalogServiceRest from "../service/CatalogServiceRest";

const CATALOG_BASE_URL = "http://localhost:5288/catalog-bff-controller";
const BASKET_BASE_URL = "http://localhost:5286/basket-bff-controller";

export const catalogService = new CatalogServiceRest(CATALOG_BASE_URL);
export const basketService = new BasketServiceRest(BASKET_BASE_URL);
export const authService = new AuthServiceClient();