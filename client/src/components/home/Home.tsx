import {useDispatch, useSelector} from 'react-redux'
import {useEffect} from 'react'
import styles from './styles'
import {CatalogItemModel} from '../../models/CatalogItemModel'
import {ItemBrandModel} from '../../models/ItemBrandModel'
import {setCatalog, setCatalogItem} from '../../redux/actions'
import {StateType} from '../../redux/store'

const Home = () => {

    const IMAGE_PATH = 'http://127.0.0.1/assets/images/'
    const items: CatalogItemModel[] = useSelector<StateType, CatalogItemModel[]>(state => state.catalog);
    const brands: ItemBrandModel[] = useSelector<StateType, ItemBrandModel[]>(state => state.brands);
    const dispatch = useDispatch<any>();

    const sorting = 0;

    const sortItems = async () => {
        if (sorting !== 0) {
            const sortedItems = [...items].sort((a, b) => sorting === 1
                ? a.price - b.price : b.price - a.price);
            await dispatch(setCatalog(sortedItems));
        }
    };

    useEffect(() => {
        sortItems();
    }, [sorting]);

    return (
        <styles.Wrapper>
            <styles.MainPageWrapper>
                {
                    items.map((e, i) => {
                        return <styles.CatalogItem key={i}>
                            <styles.NavLink
                                to={`item/${e.id}`}
                                onClick={() => dispatch(setCatalogItem(e))}
                            >
                                <styles.CardWrapper>
                                    <styles.ImageWrapper>
                                        <img src={`${IMAGE_PATH}${e.image}`} className="card-img-top" alt="..."/>
                                    </styles.ImageWrapper>

                                    <styles.CardDescription>
                                        <div>
                                            <styles.CardTitle>
                                                {brands.find((b) => b.brand == e.itemBrand)?.brand}
                                            </styles.CardTitle>
                                            <styles.CardSubTitle>
                                                {e.name}
                                            </styles.CardSubTitle>
                                        </div>
                                        <styles.CardText>
                                            {e.price} $
                                        </styles.CardText>
                                    </styles.CardDescription>
                                </styles.CardWrapper>
                            </styles.NavLink>
                        </styles.CatalogItem>
                    })
                }
            </styles.MainPageWrapper>
        </styles.Wrapper>
    )
}

export default Home

