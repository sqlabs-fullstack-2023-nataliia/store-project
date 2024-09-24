import { useEffect, useState } from 'react'
import { useDispatch } from 'react-redux';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import Main from '../../components/main/Main';
import { ROUTES } from '../../config/route-config';
import { authService, catalogService, basketService } from '../../config/service-config';
import { BasketItemModel } from '../../models/BasketItemModel';
import SideBar from '../../navigator/side-bar/SideBar';
import Navigator from '../../navigator/navigator/Navigator';
import { setUserData, setCatalog, setCategories, setTypes, setBrands, setItemsCount, setBasket } from '../../redux/actions';
import { RouteType } from '../../models/RouteType';
import styles from './styles';
import {Profile} from "../../models/Profile.ts";


const MainPage = () => {

    const dispatch = useDispatch<any>();
    //const brands: ItemBrandModel[] = useSelector<StateType, ItemBrandModel[]>(state => state.brands);
    const [category, setCategory] = useState(0);
    const [brand, setBrand] = useState('');
    const [type, setType] = useState(0);
    const [isLoading, setIsLoading] = useState(false);
    const [currentUser, setCurrentUser] = useState<Profile | null>(null);
  
    useEffect(() => {
  
      const callbackGetUser = async() => {
        const user = await authService.getUser();
        if(user){
          setCurrentUser(user)
          await dispatch(setUserData(user))
        }
      }
      callbackGetUser()
  
    }, [location.search]);
  
    const loadData = async() => {

      setIsLoading(true);
      const data = await catalogService.getCatalog({
        category: category,
        type: type,
        brand: brand
      });
      dispatch(setCatalog(data));
      setIsLoading(false)
      // TODO error handler
    }
  
    const getCategories = async() => {
      setIsLoading(true);
      const data = await catalogService.getCategories()
      dispatch(setCategories(data));
      setIsLoading(false)
      // TODO error handler
    }
  
    const getTypes = async() => {
      setIsLoading(true);
      const data = await catalogService.getTypes()
      dispatch(setTypes(data));
      setIsLoading(false)
      // TODO error handler
    }
  
    const getBrands = async() => {
      setIsLoading(true);
      const data = await catalogService.getBrands()
      dispatch(setBrands(data));
      setIsLoading(false)
      // TODO error handler
    }
  
    const setNewType = (id: number) => {
      setType(id);
    }
  
    const setNewCategory = (id: number) => {
      setCategory(id);
    }
  
    useEffect(() => {
      loadData()
    }, [type, category, brand])
  
    useEffect(() => {
      loadData();
      getCategories();
      getBrands();
      getTypes();
    }, [])
  
    useEffect(() => {
      currentUser && loadBasket()
    }, [currentUser])
  
    const loadBasket = async() => {
      setIsLoading(true)
      const basketItems = await basketService.getBasket();
      const count = basketItems.reduce((res: number, cur: BasketItemModel) => res += cur.quantity, 0);
      dispatch(setItemsCount(count))
      dispatch(setBasket(basketItems))
      setIsLoading(false)
    }
    
  return (
    <styles.Wrapper>
      <BrowserRouter >
        <Navigator
            setCategory={setNewCategory}
            setNewBrand={setBrand}
        />
        <styles.ContentWrapper>
          <SideBar setType={setNewType}/>
          <Main>
          {
            isLoading 
            ? (
            <div className='conteiner d-flex justify-content-center m-5'>
              <div className="spinner-border text-success" role="status">
              </div>
            </div>
            ) 
            : ( 
              <Routes>
                { getRoutes(ROUTES) }
              </Routes> 
            )
          }
          </Main>
        </styles.ContentWrapper>
      </BrowserRouter>
    </styles.Wrapper>
  )
}

export default MainPage;

const getRoutes = (routes: RouteType[]) => {
    return routes.map((e) => <Route key={e.path} path={e.path} element={e.element}/>)
  }
  
