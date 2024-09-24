import styled from "styled-components";

export namespace styles {

    export const Container = styled.div`
        width: 100%;
        padding: 0;
        margin: 0;
    `;

    export const FlexContainer = styled.div`
        display: flex;
        justify-content: center;
    `;

    export const Row = styled.div`
        display: flex;
        flex-wrap: wrap;
        flex-direction: row;
        justify-content: start;
        margin-bottom: 1rem;
        gap: 10px;
        width: 100%;
    `;

    export const Col = styled.div<{ lg?: number }>`
        padding: 0;
    `;
    export const Column = styled.div<{ lg?: number }>`
        padding: 0 10px;
        flex-grow: 1;
    `;

    export const Card = styled.div<{ isOrderPlaced?: boolean }>`
        width: ${({isOrderPlaced}) => (isOrderPlaced ? '10rem' : '18rem')};
        border-radius: 5px;
        height: 100%;
    `;

    export const Img = styled.img`
        width: 100%;
        height: auto;
    `;

    export const Title = styled.h1`
        color: #0d0d0d;
        text-align: left;
        width: 100%;
    `;

    export const Button = styled.button<{ quantity?: number }>`
        font-weight: bold;
        width: 40px;
        border: 1px solid #d0d0d0;
        background-color: #fff !important;
        //background: red;
    `;

    export const Total = styled.h3`
        padding: 1rem;
        text-align: left;
    `;

    export const PlaceOrderButton = styled.button`
        border: 1px solid #d0d0d0;
        background-color: #fff !important;
        height: 32px;
        width: 100px;
    `;

    export const BasketItem = styled.div`
        display: flex;
        flex-direction: row;
        justify-content: space-between;
        width: 100%;
        flex-grow: 1;
    `

    export const TotalLine = styled.div`
        display: flex;
        flex-direction: row;
        justify-content: start;
        padding: 10px;
        align-items: center;
        gap:10px;
    `
}

export default styles;