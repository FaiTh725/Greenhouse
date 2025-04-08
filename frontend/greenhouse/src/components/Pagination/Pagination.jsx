import PrimaryButton from "../buttons/PrimaryButton/PrimaryButton";
import styles from "./Pagination.module.css";

const Pagination = ({handleNext, handlePrev}) => {
  return (
    <div className={styles.Pagination__Main}>
      <div className={styles.Pagination__BtnContainer}>
        <PrimaryButton text={"◄"}
          action={handlePrev}
          backgroundColor={"#2fee9e"}
          color={"#fff"}
          hoverBackgroundColor={"#000"}
          hoverColor={"#fff"}/>
      </div>
      <div className={styles.Pagination__BtnContainer}>
        <PrimaryButton text={"►"}
          action={handleNext}
          backgroundColor={"#2fee9e"}
          color={"#fff"}
          hoverBackgroundColor={"#000"}
          hoverColor={"#fff"}/>
      </div>
    </div>
  )
}

export default Pagination;