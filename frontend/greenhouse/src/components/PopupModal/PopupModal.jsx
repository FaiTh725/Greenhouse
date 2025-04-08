import { createPortal } from "react-dom";
import PrimaryButton from "../buttons/PrimaryButton/PrimaryButton";
import styles from "./PopupModal.module.css"

const modalRoot = document.getElementById("modal-root");

const PopupModal = ({isActive, setIsActive, children}) => {
  if (typeof window === 'undefined') return null;

  return createPortal(
    <div 
      className={`${styles.PopupWindow__Main} ${isActive ? "" : styles.PopupWindow__Hiden}`}
    >
      <div className={styles.PopupWindow__Wrapper}>
        <div className={styles.PopupWindow__ActionTop}>
          <div className={styles.PopupWindow__CloseBtn}>
            <PrimaryButton text={"✖"}
              backgroundColor={"#2fee9e"}
              color={"#fff"}
              hoverBackgroundColor={"#000"}
              hoverColor={"#fff"}
              action={() => setIsActive(false)}/>
          </div>
        </div>
        <div className={styles.PopupWindow__Content}>
          {children}
        </div>
      </div>
    </div>,
    modalRoot
  );
}

export default PopupModal;