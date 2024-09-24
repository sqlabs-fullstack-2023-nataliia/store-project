import styled from "styled-components";

export namespace styles {


    export const ModalBackdrop = styled.div<{ show?: boolean }>`
        position: fixed;
        top: 0;
        left: 0;
        //display: flex;
        //justify-content: center;
        //align-items: center;
        width: 100%;
        height: 100%;
        background-color: rgba(0, 0, 0, 0.5);
        z-index: 1050;
        display: ${({show}) => (show ? 'block' : 'none')};
    `;

    export const ModalDialog = styled.div`
        display: flex;
        justify-content: center;
        align-items: center;
        min-width: 80%;
        max-width: 100%;
        height: 100%;
        //height: 400px;
        //overflow-y: auto;

    `;

    export const ModalContent = styled.div`
        background-color: #fff;
        border-radius: 0.3rem;
        box-shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.5);
        //overflow: hidden;
        //height: 500px;
        //overflow-y: auto;
    `;

    export const ModalHeader = styled.div`
        padding: 1rem;
        border-bottom: 1px solid #dee2e6;
        display: flex;
        justify-content: flex-end;
    `;

    export const CloseButton = styled.button`
        background: none;
        border: none;
        font-size: 1.5rem;
        cursor: pointer;
    `;

    export const ModalBody = styled.div`
        padding: 1rem;
        display: flex;
        flex-direction: column;

        height: 500px;
        overflow-y: auto;
    `;

    export const ModalFooter = styled.div`
        padding: 1rem;
        border-top: 1px solid #dee2e6;
        display: flex;
        justify-content: flex-end;
    `;

    export const ButtonSecondary = styled.button`
        background-color: #6c757d;
        color: white;
        border: none;
        padding: 0.375rem 0.75rem;
        border-radius: 0.25rem;
        cursor: pointer;

        &:hover {
            background-color: #5a6268;
        }
    `;
}

export default styles;