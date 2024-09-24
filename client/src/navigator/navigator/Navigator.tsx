import { BASKET_PATH, HOME_PATH, LOGIN_PATH, LOGOUT_PATH } from '../../config/route-config'
import { FaShoppingBasket } from "react-icons/fa";
import { ItemCategoryModel } from '../../models/ItemCategoryModel';
import { useState } from 'react';
import { Link } from 'react-router-dom';
import { useSelector } from 'react-redux';
import { StateType } from '../../redux/store';
import styles from './styles';
import { IoIosClose } from "react-icons/io";
import {Profile} from "../../models/Profile.ts";


interface Props {
    setCategory: (id: number) => void
    setNewBrand: (id: string) => void
}
const Navigator = ({setCategory, setNewBrand}: Props) => {

  const [brand, setBrand] = useState('');

  const categories: ItemCategoryModel[] = useSelector<StateType, ItemCategoryModel[]>(state => state.categories);
  const count: number = useSelector<StateType, number>(state => state.count);
  const user: Profile | null = useSelector<StateType, Profile | null>(state => state.userData);

  const handleBrand = async() => {
      if(!brand) {
          setNewBrand(brand)
          // return;
      }
      setNewBrand(brand)
  }

  return (
     <>
      <styles.Wrapper>
      <styles.NavigatorWrapper>
        <styles.List>
             {
                categories.map((e) => {
                    return <styles.ListItem
                        key={e.id} 
                        className="nav-item" 
                        >
                        <styles.LinkWrapper>
                        <styles.StyledLink
                            to={HOME_PATH} 
                            onClick={() => setCategory(e.id)}
                        >
                           {e.category}
                          </styles.StyledLink>
                        </styles.LinkWrapper>
                      
                      
                        </styles.ListItem>
                })
              }
        </styles.List>
        <styles.List>
         {
            user?.access_token
            ? (<>
            <styles.ListItem>
                {user?.email}
            </styles.ListItem>
            <styles.ListItem>
                <styles.StyledLink
                    to={LOGOUT_PATH} 
                    >
                    LOGOUT
                </styles.StyledLink>
            </styles.ListItem>
            </>) 
            : (<styles.ListItem>
                <styles.StyledLink
                    to={LOGIN_PATH} 
                    >
                    LOGIN
                </styles.StyledLink>
            </styles.ListItem>)
        }
        <styles.BasketIconWrapper>
            <Link
                to={BASKET_PATH}
                style={{display:"flex", flexDirection: "row", gap: 10}}
                >
                <FaShoppingBasket style={{color: "#d0d0d0"}} />
                {
                count !== 0 && <span className="badge badge-success" style={{background: 'green', borderRadius: '25px'}}>{count}</span>
            }
            </Link>
        </styles.BasketIconWrapper>
        </styles.List>
     </styles.NavigatorWrapper>

     <styles.FormWrapper>
      <styles.Form>
        <styles.Input
          onChange={(e) => {setBrand(e.target.value)}} 
          placeholder="brand" 
          aria-label="Search"
          value={brand}
        />
        <styles.ButtonLink
          to={HOME_PATH} 
          onClick={handleBrand} 
        >
          Search
        </styles.ButtonLink>
        <styles.SelectedBrandWrapper>
        {brand
            && <>
              <styles.BrandWrapper>
              <styles.Lable>
                  {brand}
              </styles.Lable>
              <styles.CloseIconErapper>
                <IoIosClose onClick={() => setBrand("")} style={{cursor: 'pointer'}}/>
              </styles.CloseIconErapper>
              </styles.BrandWrapper>
            </>}
        </styles.SelectedBrandWrapper>
      </styles.Form>
     </styles.FormWrapper>
    </styles.Wrapper>
  </>
  )
}

export default Navigator

//     {/* <a className="navbar-brand" href={HOME_PATH}><span style={{color: 'white', fontWeight: 'bold', }}>STO</span><span style={{color: '#1a903e', fontWeight: 'bold', fontSize: '25px'}}>RE</span></a>
//     <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation" style={{background: 'white'}}>
//         <span className="navbar-toggler-icon"></span>
//     </button> */}




