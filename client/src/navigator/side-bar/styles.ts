import styled from "styled-components";
import {Link} from "react-router-dom";

export namespace styles {

    export const Wrapper = styled.div`
        //padding: 10px;
    `;

    export const StyledList = styled.ul`
  list-style-type: none;
  padding: 0;
`;

    export const StyledListItem = styled.li<{ selected: boolean }>`
  cursor: pointer;
  background-color: ${(props) => (props.selected ? '#f0f0f0' : 'transparent')};
  //padding: 10px;
  //margin: 5px 0;
  // border: ${(props) => (props.selected ? '2px solid #007bff' : '1px solid #ccc')};
  //border-radius: 5px;
        
        &:hover {
            border-radius: 10px;
        }
`;

    export const StyledLink = styled(Link)<{ selected: boolean }>`
  text-decoration: none;
  color: ${(props) => (props.selected ? '#fff' : '#0d0d0d')};
  background-color: ${(props) => (props.selected ? '#0d0d0d' : 'transparent')};
  padding: 10px;
  font-size: 15px;
  font-weight: bold;
  display: block;
  width: 100%;
  &:hover {
    color: ${(props) => (props.selected ? '#fff' : '#0056b3')};
  }
`;
}

export default styles;