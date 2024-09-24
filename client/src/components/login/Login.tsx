import {useDispatch} from "react-redux";
import {useNavigate} from "react-router-dom";
import {useState} from "react";
import {SubmitHandler, useForm} from "react-hook-form";
import {authService} from "../../config/service-config.ts";
import styles from './styles';
import {setUserData} from "../../redux/actions.ts";
import {SignUpData} from "../../models/auth/SignUpData.ts";

type Inputs = {
  email: string,
  password: string,
  firstName: string,
  lastName: string
}

const OK_STATUS_CODE = 200;
const BAD_REQUEST_STATUS_CODE = 400;
const ACCOUNT_CREATED_MESSAGE = 'New account was successfily created. Please log in!';
const WRONG_CREDENTIALS_MESSAGE = 'Wrong Credentials';
const ERROR_MESSAGE = 'Error occurred'

const Login = () => {

  const dispatch = useDispatch<any>();
  const navigate = useNavigate()
  const [isLogin, setIsLogin] = useState(true);
  const [isLoading, setIsLoading] = useState(false);
  const [showError, setShowError] = useState(false);
  const [message, setMessage] = useState('');

  const {
    reset,
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<Inputs>()
  const onSubmit: SubmitHandler<Inputs> = (data) => isLogin ? handleLogin(data) : handleSignUp(data)

  const handleLogin = async (data: any) => {
    setIsLoading(true)
    const res = await authService.login(
        {
          email: data.email,
          password: data.password
        }
    )
    if(res){
      dispatch(setUserData(res))
      reset()
      navigate(-1)
      setShowError(false);
    } else {
      setMessage(WRONG_CREDENTIALS_MESSAGE)
      setShowError(true);
      // toast.error(WRONG_CREDENTIALS_MESSAGE, {
      //   position: "top-center",
      //   autoClose: 2000,
      //   hideProgressBar: false,
      //   closeOnClick: true,
      //   pauseOnHover: true,
      //   draggable: false,
      //   theme: "light"
      // })
    }
    setIsLoading(false)
  }

  console.log(showError)
  const handleSignUp = async (data: any) => {
    console.log(data)
    // let message = ACCOUNT_CREATED_MESSAGE;
    setMessage(ACCOUNT_CREATED_MESSAGE)
    setIsLoading(true)
    const res = await authService.signup(data as SignUpData)
    if(res === OK_STATUS_CODE){
      reset()
      setShowError(false)
      // toast.success(message, {
      //   position: "top-center",
      //   autoClose: 2000,
      //   hideProgressBar: false,
      //   closeOnClick: true,
      //   pauseOnHover: true,
      //   draggable: false,
      //   theme: "light"
      // })
      setIsLogin(!isLoading)
    } else {
      if(res === BAD_REQUEST_STATUS_CODE){
        setMessage(WRONG_CREDENTIALS_MESSAGE)
        setShowError(true)
        // message = WRONG_CREDENTIALS_MESSAGE
      } else {
        setMessage(ERROR_MESSAGE)
        setShowError(true)
        // message = ERROR_MESSAGE
      }
      // toast.error(message, {
      //   position: "top-center",
      //   autoClose: 2000,
      //   hideProgressBar: false,
      //   closeOnClick: true,
      //   pauseOnHover: true,
      //   draggable: false,
      //   theme: "light",
      // })
    }

    setIsLoading(false)
  }


  return (
      <>
        {/*<ToastContainer*/}
        {/*    position="top-right"*/}
        {/*    autoClose={5000}*/}
        {/*    hideProgressBar={false}*/}
        {/*    newestOnTop={false}*/}
        {/*    closeOnClick*/}
        {/*    rtl={false}*/}
        {/*    pauseOnFocusLoss*/}
        {/*    draggable*/}
        {/*    pauseOnHover*/}
        {/*/>*/}
        <styles.Container>
          <styles.FormStack>
            <styles.FormBox>
              <styles.Heading>
                {isLogin ? "Login" : "Signup"}
              </styles.Heading>
            </styles.FormBox>
            {isLoading ? (
                <styles.SpinnerBox>
                  <div className="spinner" />
                </styles.SpinnerBox>
            ) : (
                <form onSubmit={handleSubmit(onSubmit)}>
                  {!isLogin && (
                      <>
                        <styles.FormControl>
                          <styles.FormLabel>First Name</styles.FormLabel>
                          <styles.Input type='text' {...register("firstName", {required: true, minLength: 2})}/>
                          {errors.firstName ? (
                              <styles.ErrorMessage>First Name is required.</styles.ErrorMessage>
                          ) : (
                              <styles.HelperText>Please enter your first name.</styles.HelperText>
                          )}
                        </styles.FormControl>
                        <styles.FormControl>
                          <styles.FormLabel>Last Name</styles.FormLabel>
                          <styles.Input type='text' {...register("lastName", {required: true, minLength: 2})}/>
                          {errors.lastName ? (
                              <styles.ErrorMessage>Last Name required.</styles.ErrorMessage>
                          ) : (
                              <styles.HelperText>Please enter your last name.</styles.HelperText>
                          )}
                        </styles.FormControl>
                      </>
                  )}
                  <styles.FormControl>
                    <styles.FormLabel>Email address</styles.FormLabel>
                    <styles.Input type='email' {...register("email", {required: true})}/>
                    {errors.email ? (
                        <styles.ErrorMessage>Email is required.</styles.ErrorMessage>
                    ) : (
                        <styles.HelperText>Please enter your email.</styles.HelperText>
                    )}
                  </styles.FormControl>
                  <styles.FormControl>
                    <styles.FormLabel>Password</styles.FormLabel>
                    <styles.Input type='password' {...register("password", {required: true, minLength: 8})}/>
                    {errors.password ? (
                        <styles.ErrorMessage>Password is required.</styles.ErrorMessage>
                    ) : (
                        <styles.HelperText>Please enter your password.</styles.HelperText>
                    )}
                  </styles.FormControl>
                  {
                      showError && <styles.ErrorWrapper>{message}</styles.ErrorWrapper>
                  }
                  {/*<styles.ErrorWrapper>{message}</styles.ErrorWrapper>*/}
                  <styles.ButtonsWrapper>
                    <styles.Button type='submit'>{isLogin ? "Login" : "Signup"}</styles.Button>
                    <styles.Button onClick={() => setIsLogin(!isLogin)} color='teal'>
                      {isLogin ? "Signup" : "Login"}
                    </styles.Button>
                  </styles.ButtonsWrapper>
                </form>
            )}
          </styles.FormStack>
        </styles.Container>
      </>
  )
}

export default Login;

