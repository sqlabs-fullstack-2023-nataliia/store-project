import styled from "styled-components";
import { Link } from 'react-router-dom';
import backgroundFone from '../../../public/images/bg-image.png';

export namespace styles {

    export const NavigatorWrapper = styled.div`
        padding: 0 10px;
        display: flex;
        justify-content: space-between;
        height: 35px;
    `;

    export const List = styled.ul`
        margin: 0;
        padding:0;
        display: flex;
        align-items: start;
        justify-content: start;
        flex-direction: row;
        list-style: none;
        /* color: white; */
        gap: 10px;
    `;

    export const BasketIconWrapper = styled.li`
        display: flex;
        width: 100%;
        height: 100%;
        justify-content: center;
        align-items: center;
    `

    export const ListItem = styled.li`
        margin: 0;
        width: 100%;
        height: 100%;
        display: flex;
        align-items: center;

        :hover {
            background-color: #e1e2eb;
            color: rgb(27, 27, 27);
            border-radius: 15px;
    }
    `;

    export const LinkWrapper = styled.div`
        width: 100%;
        height: 100%;
        display: flex;
        align-items: center;
    `;

    export const StyledLink = styled(Link)`
        color: #0d0d0d;
        font-size: 12px;
        font-weight: 400;
        text-decoration: none;
        padding: 0 10px;
        width: 100%;
        height: 100%;
        display: flex;
        align-items: center;
    `;

    export const Form = styled.div`
        display: flex;
        align-items: center;
        gap: 15px;
    `;

    export const Input = styled.input`
        border-radius: 10px;
        padding: 2px 15px;
        background-color: white;
        width: 150px;
        border: 1px solid #d0d0d0;

        &::placeholder {
            color: rgb(37, 37, 37);
            opacity: 0.5;
            font-size: 14px;
        }

        &:hover {

        }

        &:focus {
            outline: none; 
        }

        &:active {
        }
    `;

    export const ButtonLink = styled(Link)`
        font-size: 12px;
        font-weight: 500;
        text-decoration: none;
        width: 60px;
        padding: 1px 10px;
        border-radius: 10px;
        background-color: #d0d0d0;
        color: #ffffff;
        
        &:hover {
            color: rgb(27, 27, 27);
            background-color: aliceblue;
            padding: 2px 10px;
            
        }

        &:active {
            color: rgb(27, 27, 27);
            background-color: white;
            padding: 2px 10px;
            border-radius: 10px;
        }
    `;

    export const FormWrapper = styled.div`
        padding: 10px 10px;
        display: flex;
        justify-content: space-between;
    `;

    export const Wrapper = styled.div`
        /* width: 100%;
        opacity: 50%; */
        /* background-image: url(${backgroundFone}); */
    `;

    export const SelectedBrandWrapper = styled.div`

    `;

    export const BrandWrapper = styled.div`
        background-color: white;
        display: flex;
        justify-content: center;
        align-items: center;
        flex-direction: row;
        padding: 2px 10px;
        gap: 5px;
        border-radius: 15px;
    `;

    export const Lable = styled.span`
        font-size: 12px;
        font-weight: 500;
        color: rgb(27, 27, 27);
    `;

    export const CloseButton = styled.span`
    `;

    export const CloseIconErapper = styled.span`
        display: flex;
        align-items: center;
        &:hover {
            color: #420615;
            background-color: #e0e0e0;
            border-radius: 25px;
            
        }

        &:active {
            color: #ba002f;
            background-color: #cfcfcf;
            border-radius: 25px;

        }

    `;
}

export default styles;