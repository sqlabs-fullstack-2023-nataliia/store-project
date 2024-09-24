import Basket from '../basket/Basket';
import styles from './syles.ts'

interface Props {
    show: boolean
    onClose: () => void

}
const BasketModal = ({show, onClose}: Props) => {

    return (
        <styles.ModalBackdrop show={show}>
            <styles.ModalDialog>
                <styles.ModalContent>
                    <styles.ModalHeader>
                        <styles.CloseButton onClick={onClose} aria-label="Close">&times;</styles.CloseButton>
                    </styles.ModalHeader>
                    <styles.ModalBody>
                        <div className="row">
                            {show && <Basket isModal={true} />}
                        </div>
                    </styles.ModalBody>
                    <styles.ModalFooter>
                        <styles.ButtonSecondary onClick={onClose}>Close</styles.ButtonSecondary>
                    </styles.ModalFooter>
                </styles.ModalContent>
            </styles.ModalDialog>
        </styles.ModalBackdrop>
    )
}

export default BasketModal
