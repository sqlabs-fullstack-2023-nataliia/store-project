import styled from "styled-components";
import backgroundFone from '../../../public/images/bg-image.png'

export namespace styles {

    export const Wrapper = styled.div`
        width: 100%;
        height: 100%;
        min-height: 100vh;
        z-index: 1;
        /* &::before {
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-image: url(${backgroundFone});
            background-size: cover;
            background-position: center;
            opacity: 0.5;
            z-index: -1;
        } */
    `;

    export const ContentWrapper = styled.div`
        display: flex;
        flex-direction: row;
        gap: 20px;
        height: 100%;
        overflow: auto;
        /* height: 85vh; */
    `;

}

export default styles;