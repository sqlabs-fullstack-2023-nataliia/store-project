import { useEffect, useState } from 'react'
import './App.css'
import { useDispatch} from 'react-redux';
import { authService, basketService } from './config/service-config';
import { setBasket, setItemsCount, setUserData } from './redux/actions';
import { BasketItemModel } from './models/BasketItemModel';
import MainPage from './pages/main-page/MainPage';
import {Profile} from "./models/Profile.ts";



function App() {
  const dispatch = useDispatch<any>();
  const [currentUser, setCurrentUser] = useState<Profile | null>(null);

  useEffect(() => {

    const callbackGetUser = async() => {
      const user = await authService.getUser();
      if(user){
        setCurrentUser(user)
        console.log('App *** ' + user.access_token)
        await dispatch(setUserData(user))
      }
    }
    callbackGetUser()

  }, [location.search]);

  useEffect(() => {
    currentUser && loadBasket()
  }, [currentUser])

  const loadBasket = async() => {
    const basketItems = await basketService.getBasket();
    const count = basketItems.reduce((res: number, cur: BasketItemModel) => res += cur.quantity, 0);
    dispatch(setItemsCount(count))
    dispatch(setBasket(basketItems))
  }

  return (
      <>
        {/*<ToastContainer />*/}
        <MainPage/>
      </>
  )
}

export default App




