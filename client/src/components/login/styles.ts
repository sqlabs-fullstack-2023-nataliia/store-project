import styled from "styled-components";

export namespace styles {

    export const Container = styled.div`
        display: flex;
        width: 100%;
        justify-content: center;
    `;

    export const FormStack = styled.div`
  display: flex;
  flex-direction: column;
  gap: 1rem;
  width: 300px;
`;

    export const Heading = styled.h2`
  font-size: 2rem;
  text-align: center;
`;

    export const FormBox = styled.div`
  display: flex;
  justify-content: center;
`;

    export const SpinnerBox = styled.div`
  display: flex;
  justify-content: center;
`;

    export const FormControl = styled.div`
  margin: 1rem 0;
`;

    export const ErrorWrapper = styled.div`
        height: 30px;
        color: red;
        display: flex;
        justify-content: center;
    `

    export const FormLabel = styled.label`
  margin-bottom: 0.5rem;
  display: block;
`;

    export const Input = styled.input`
  padding: 0.5rem;
  border: 1px solid #ccc;
  width: 100%;
  border-radius: 4px;
`;

    export const Button = styled.button`
  background-color: ${props => props.color || '#3182ce'};
  color: white;
  padding: 0.75rem;
  width: 100%;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  
  &:hover {
    background-color: #2b6cb0;
  }
`;

    export const ButtonsWrapper = styled.div`
        display: flex;
        width: 100%;
        flex-wrap: wrap;
        gap: 15px;
    `;

    export const ErrorMessage = styled.p`
  color: red;
  font-size: 0.875rem;
`;

    export const HelperText = styled.p`
  font-size: 0.875rem;
  color: gray;
`;
}

export default styles;