import styled from "styled-components";

export namespace styles {

    export const Container = styled.div`
        display: flex;
        justify-content: center;
        align-items: center;
    `;

    export const Card = styled.div`
        border-radius: 5px;
        width: 90%;
        box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
        display: flex;
        flex-direction: row;
    `;

    export const HeadLine= styled.div`
        display: flex; 
        flex-direction: row;
        justify-content: space-between;
    `

    export const CardImage = styled.img`
        width: 100%;
        border-top-left-radius: 5px;
        border-top-right-radius: 5px;
    `;

    export const CardBody = styled.div`
        padding: 20px;
    `;

    export const Title = styled.h3`
        text-align: center;
        text-transform: uppercase;
        margin-bottom: 10px;
    `;

    export const SubTitle = styled.h5`
        text-align: center;
        text-transform: uppercase;
        margin-bottom: 15px;
    `;

    export const Price = styled.p`
        text-align: right;
        font-weight: bold;
    `;

    export const Description = styled.p`
        text-align: left;
        text-transform: uppercase;
        margin-top: 20px;
    `;

    export const Select = styled.select`
        width: 100%;
        padding: 8px;
        border-radius: 5px;
        border: 1px solid #d0d0d0;
        margin-top: 15px;
    `;

    export const Button = styled.button<{ disabled?: boolean }>`
        width: 100%;
        padding: 10px;
        border: 1px solid #d0d0d0;
        background-color: #fff !important;
        margin-top: 20px;
        cursor: pointer;

        &:disabled {
            opacity: 0.9;
            border-color: gray;
            color: gray;
            cursor: not-allowed;
        }
    `;

    export const BasketModalWrapper = styled.div`
        margin-top: 30px;
    `;
}

export default styles;