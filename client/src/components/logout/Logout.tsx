import { useEffect } from 'react'
import { useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { HOME_PATH } from '../../config/route-config';
import { authService } from '../../config/service-config';
import { setUserData } from '../../redux/actions';

const Logout = () => {

  const navigate = useNavigate();
  const dispatch = useDispatch();

  useEffect(() => {
    logoutUser();
  }, [])

  const logoutUser = async () => {
    await authService.logout();
    dispatch(setUserData(null))
    navigate(HOME_PATH);
  }
  
  return (
    <></>
  )
}

export default Logout
