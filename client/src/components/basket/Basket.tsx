import {useDispatch, useSelector} from 'react-redux';
import {useState, useEffect, useCallback} from 'react';
import {basketService} from '../../config/service-config';
import {BasketItemModel} from '../../models/BasketItemModel';
import {setBasket, setItemsCount} from '../../redux/actions';
import {StateType} from '../../redux/store';
import {Profile} from "../../models/Profile.ts";
import styles from './styles.ts'

interface Props {
    isModal?: boolean
}

const Basket = ({isModal}: Props) => {

    const IMAGE_PATH = 'http://127.0.0.1/assets/images/'

    const dispatch = useDispatch<any>();
    const [items, setItems] = useState<BasketItemModel[]>([]);
    const [orderId, setOrderId] = useState(0);
    const [isOrderPlaced, setIsOrderPlased] = useState(false);
    const user: Profile | null = useSelector<StateType, Profile | null>(state => state.userData)

    const addToBasket = async (id: number) => {
        console.log('add to basket')
        if (user) {
            await basketService.addToBasket(id);
            await loadBasket();
        }
    }

    const removeFromBasket = async (id: number) => {
        if (user) {
            await basketService.removeFromBasket(id);
            await loadBasket()
        }
    }

    const checkoutBasket = async () => {
        if (user) {
            const orderId = await basketService.checkoutBasket();
            console.log(orderId)
            setOrderId(orderId)
            setIsOrderPlased(true);
        }
    }

    const loadBasket = useCallback(async () => {
        if (user) {
            const basketItems = await basketService.getBasket();
            setItems(basketItems);
            await dispatch(setBasket(basketItems));
            const count = basketItems.reduce((res: number, cur: BasketItemModel) => res += cur.quantity, 0);
            await dispatch(setItemsCount(count));
        }
    }, [dispatch, user]);

    useEffect(() => {
        loadBasket().catch(console.error);
    }, [loadBasket]);

    return (
        <styles.Container>
            <styles.FlexContainer>
                {items.length === 0 ? (
                    <styles.Title>Basket empty</styles.Title>
                ) : (
                    <>
                        {isOrderPlaced ? (
                            <styles.Title>Your order confirmation number: {orderId}</styles.Title>
                        ) : (
                            <styles.Title>Check Out</styles.Title>
                        )}
                    </>
                )}
            </styles.FlexContainer>
            {items.map((e, i) => (
                <styles.Row key={i}>
                    <styles.Col lg={isModal ? undefined : 5}>
                        <styles.Card isOrderPlaced={isOrderPlaced}>
                            <styles.Img src={`${IMAGE_PATH}${e.image}`}/>
                        </styles.Card>
                    </styles.Col>
                    <styles.Column lg={isModal ? undefined : 5}>
                        <styles.BasketItem>
                            <div>
                                <h6 style={{fontWeight: 'bold'}}>{e.name}</h6>
                                <p>
                                    <span style={{fontWeight: 'bold'}}>Size: </span>{e.size}
                                </p>
                                <p>
                                    <span style={{fontWeight: 'bold'}}>Price: </span>{e.price}$
                                </p>
                                <p>
                                    <span style={{fontWeight: 'bold'}}>Quantity: </span>{e.quantity}
                                </p>
                                <p>
                                    <span style={{fontWeight: 'bold'}}>Subprice: </span>
                                    {(e.price * e.quantity).toLocaleString('en-US', {
                                        minimumFractionDigits: 2,
                                        maximumFractionDigits: 2,
                                    })}{' '}
                                    $
                                </p>
                            </div>
                            {!isOrderPlaced && (
                                <styles.Col lg={isModal ? undefined : 3}>
                                    <styles.Button onClick={() => removeFromBasket(e.itemId)} disabled={e.quantity < 0}
                                                   quantity={e.quantity}>
                                        -
                                    </styles.Button>
                                    <span className="p-3"
                                          style={{fontWeight: 'bold', fontSize: '20px'}}>{e.quantity}</span>
                                    <styles.Button
                                        onClick={() => addToBasket(e.itemId)}
                                        //disabled={e.stockQuantity - e.quantity < 1}
                                        //quantity={e.quantity}
                                    >
                                        +
                                    </styles.Button>
                                </styles.Col>
                            )}
                        </styles.BasketItem>
                    </styles.Column>
                </styles.Row>
            ))}
            {items.length > 0 && (
                <styles.TotalLine>
                            <styles.Total>
                                Total:{' '}
                                {items.reduce((res, cur) => res + cur.price * cur.quantity, 0).toLocaleString('en-US', {
                                    minimumFractionDigits: 2,
                                    maximumFractionDigits: 2,
                                })}{' '}
                                $
                            </styles.Total>
                        {!isOrderPlaced && (
                            <div><styles.PlaceOrderButton onClick={checkoutBasket}>Place Order</styles.PlaceOrderButton></div>
                        )}
                </styles.TotalLine>
            )}
        </styles.Container>
    )
}

export default Basket
