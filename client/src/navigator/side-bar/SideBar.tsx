import { useState } from 'react';
import { useSelector } from 'react-redux';
import { HOME_PATH } from '../../config/route-config';
import { ItemTypeModel } from '../../models/ItemTypeModel';
import { StateType } from '../../redux/store';
import styles from './styles';

interface Props {
    setType: (id: number) => void
}

const SideBar = ({setType}: Props) => {

    const [selectedType, setSelectedType] = useState<number>(0);
    const types: ItemTypeModel[] = useSelector<StateType, ItemTypeModel[]>(state => state.types);

    return (
        <styles.Wrapper>
            <styles.StyledList>
                <styles.StyledListItem
                    selected={selectedType === 0}
                    onClick={() => setSelectedType(0)}
                >
                    <styles.StyledLink
                        selected={selectedType === 0}
                        to={HOME_PATH}
                        onClick={() => setType(0)}
                    >
                        All types
                    </styles.StyledLink>
                </styles.StyledListItem>
                {types.map((type) => (
                    <styles.StyledListItem
                        key={type.id}
                        selected={selectedType === type.id}
                        onClick={() => setSelectedType(type.id)}
                    >
                        <styles.StyledLink
                            selected={selectedType === type.id}
                            to={HOME_PATH}
                            onClick={() => setType(type.id)}
                        >
                            {type.type}
                        </styles.StyledLink>
                    </styles.StyledListItem>
                ))}
            </styles.StyledList>
        </styles.Wrapper>

  )
}

export default SideBar
