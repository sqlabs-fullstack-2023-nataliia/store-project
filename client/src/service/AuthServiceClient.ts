import axios from "axios";
import {emptyClientData, Profile} from "../models/Profile.ts";
import {SignUpData} from "../models/auth/SignUpData.ts";
import {LoginData} from "../models/auth/LoginData.ts";

export const CLIENT_DATA_ITEM = "CLIENT_DATA_ITEM";

export const BASE_URL = 'http://localhost:3001/auth'
export default class AuthServiceClient {// implements AuthService {

    async login(loginData: LoginData): Promise<Profile | null> {
        return await axios.post(`${BASE_URL}/login`, loginData)
            .then(res => {
                localStorage.setItem(CLIENT_DATA_ITEM, JSON.stringify(res.data));
                return res.data
            })
            .catch(err => {
                console.log(err.message)
                return null
            })

    }

    async signup(signUpData: SignUpData): Promise<number> {
        return await axios.post(`${BASE_URL}/signup`, signUpData)
            .then(res => {
                console.log(res)
                return 200
            })
            .catch(err => {
                console.log(err.message)
                return err.response.status;
            })

    }

    getUser(): Profile {
        const user = localStorage.getItem(CLIENT_DATA_ITEM);
        if (user) {
            try {
                return JSON.parse(user) as Profile;
            } catch (error) {
                console.error('Error parsing user data from localStorage:', error);
                return emptyClientData;
            }
        }
        return emptyClientData;
    }


    async logout(): Promise<boolean> {
        localStorage.setItem(CLIENT_DATA_ITEM, JSON.stringify(emptyClientData));
        return true
    }

  }
    
