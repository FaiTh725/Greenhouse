import PrimaryButton from "../buttons/PrimaryButton/PrimaryButton";
import styles from "./ConfirmUser.module.css"

const ConfirmUserCart = ({user, handleConfirm}) => {
  return (
    <div className={styles.ConfirmUserCart__Main}>
      <div className={styles.ConfirmUserCart__Wrapper}>
        <p className={styles.ConfirmUserCart__Email}>{user.email}</p>
        <p className={styles.ConfirmUserCart__Name}>{user.name}</p>
        <div className={styles.ConfirmUserCart__Btn}>
          <div>
            <PrimaryButton 
              action={() => handleConfirm(user.email)}
              text={"✔"}
              backgroundColor={"#2fee9e"}
              color={"#fff"}
              hoverBackgroundColor={"#000"}
              hoverColor={"#fff"}/>
          </div>
        </div>
        <div className={styles.ConfirmUserCart__Btn}>
          <div>
            <PrimaryButton 
              text={"✖"}
              backgroundColor={"#d43d65"}
              color={"#fff"}
              hoverBackgroundColor={"#000"}
              hoverColor={"#fff"}/>
          </div>
        </div>
      </div>
    </div>
  )
}

export default ConfirmUserCart;