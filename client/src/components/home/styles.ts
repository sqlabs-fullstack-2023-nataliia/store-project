import styled from "styled-components";
import { Link } from 'react-router-dom'

export namespace styles {

    export const Wrapper = styled.div`
        width: 100%;
        display: flex;
    `;

    export const MainPageWrapper = styled.div`
        display: flex;
        flex-direction: row;
        flex-wrap: wrap;
        gap: 15px;
        width: 100%;
        max-height: 85vh;
        overflow-y: auto;
    `;

    export const CardWrapper = styled.div`
        width: 250px;
        padding: 10px;
        &:hover {
            box-shadow: rgba(50, 50, 93, 0.25) 0px 6px 12px -2px, rgba(0, 0, 0, 0.3) 0px 3px 7px -3px;
        }
    `;

    export const CardDescription = styled.div`
        display: flex;
        flex-direction: row;
        justify-content: space-between;
        padding: 10px 0;
    `;

    export const CardTitle = styled.div`
    
    `;

    export const CardSubTitle = styled.div`
    
    `;

    export const CardText = styled.div`
    
    `;

    export const ImageWrapper = styled.div`
        width: 100%;
        height: 100%;
        background-color: #fff;
    `;

    export const CatalogItem = styled.div`
        display: flex;
        flex-direction: row;
        gap: 20px;
        padding: 0px 20px;
    `

    export const NavLink = styled(Link)`
        text-decoration: none;
        color: #0d0d0d;
        &:hover {
            text-decoration: underline;
        }
    `
}

export default styles;