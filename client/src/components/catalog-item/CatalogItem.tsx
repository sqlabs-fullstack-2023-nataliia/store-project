import { ItemBrandModel } from "../../models/ItemBrandModel";
import { useEffect, useState } from "react";
import { ItemModel } from "../../models/ItemModel";
import { useDispatch, useSelector } from "react-redux";
import { StateType } from "../../redux/store";
import { basketService, catalogService } from "../../config/service-config";
import { CatalogItemModel } from "../../models/CatalogItemModel";
import { setBasket, setItemsCount } from "../../redux/actions";
import { LOGIN_PATH } from "../../config/route-config";
import { useNavigate } from "react-router-dom";
import { useParams } from 'react-router-dom';
import BasketModal from "../basket-modal/BasketModal";
import { Profile } from "../../models/Profile";
import styles from './styles.ts';

const CatalogItem = () => {
    const IMAGE_PATH = 'http://127.0.0.1/assets/images/';

    const dispatch = useDispatch<any>();
    const brands: ItemBrandModel[] = useSelector<StateType, ItemBrandModel[]>(state => state.brands);
    const count: number = useSelector<StateType, number>(state => state.count);
    const user: Profile | null = useSelector<StateType, Profile | null>(state => state.userData);

    const [item, setItem] = useState<ItemModel>();
    const [items, setItems] = useState<ItemModel[]>([]);
    const [catalogItem, setCatalogItem] = useState<CatalogItemModel>();
    const [loading, setLoading] = useState<boolean>(true);

    const [isModalOpen, setIsModalOpen] = useState(false);

    const openModal = () => {
        setIsModalOpen(true);
    };

    const closeModal = () => {
        setIsModalOpen(false);
    };

    const navigate = useNavigate();
    const { itemId } = useParams();

    const loadItem = async () => {
        if (itemId) {
            try {
                const currentCatalogItem = await catalogService.getCatalogItem(+itemId);
                if (currentCatalogItem) {
                    setCatalogItem(currentCatalogItem);
                    const currentItems = await catalogService.getItems(currentCatalogItem.id);
                    currentItems && setItems(currentItems);
                }
            } catch (error) {
                console.error('Error loading catalog item:', error);
            } finally {
                setLoading(false);
            }
        }
    };

    useEffect(() => {
        loadItem();
    }, [itemId]);

    const handleAdding = async () => {
        if (item && item.quantity > 0 && user?.access_token) {
            console.log('Adding item to the basket', item);
            try {
                await basketService.addToBasket(item.id);
                dispatch(setItemsCount(count + 1));
                const basketItems = await basketService.getBasket();
                dispatch(setBasket(basketItems));
                openModal();
            } catch (error) {
                console.error('Failed to add item to basket', error);
            }
        } else if (!user?.access_token) {
            console.log('User not logged in, redirecting to login page');
            navigate(LOGIN_PATH);
        } else {
            console.log('Item quantity is zero or not selected');
        }
    };

    if (loading) {
        return <div>Loading...</div>; // Placeholder for loading state
    }

    return (
        <styles.Container>
            {catalogItem !== undefined && (
                <styles.Card>
                    <styles.CardImage src={`${IMAGE_PATH}${catalogItem.image}`} alt={catalogItem.name} />
                    <styles.CardBody>
                        <styles.HeadLine>
                        <styles.Title>
                            {brands.find((b) => b.brand === catalogItem.itemBrand)?.brand}
                        </styles.Title>
                        <styles.SubTitle>{catalogItem.name}</styles.SubTitle>
                        <styles.Price>{catalogItem.price} $</styles.Price>
                        </styles.HeadLine>
                        <styles.Select
                            defaultValue="0"
                            onChange={(e) => {
                                const selectedItem = items.find((item) => item.id + '' === e.target.value);
                                if (selectedItem) {
                                    setItem(selectedItem);
                                } else {
                                    console.error('Selected item not found');
                                }
                            }}
                        >
                            <option value="0" disabled>
                                Select size
                            </option>
                            {items.map((e) => (
                                <option value={e.id + ''} key={e.id + ''} disabled={e.quantity === 0}>
                                    {e.size}
                                </option>
                            ))}
                        </styles.Select>
                        <styles.Description>{catalogItem.description}</styles.Description>
                        <styles.Button
                            onClick={handleAdding}
                            disabled={!item}
                        >
                            Add To Basket
                        </styles.Button>
                    </styles.CardBody>
                </styles.Card>
            )}
            <styles.BasketModalWrapper>
                <BasketModal show={isModalOpen} onClose={closeModal}/>
            </styles.BasketModalWrapper>
        </styles.Container>
    );
};

export default CatalogItem;



