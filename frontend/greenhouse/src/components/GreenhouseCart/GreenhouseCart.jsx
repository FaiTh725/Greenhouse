import styles from "./GreenhouseCart.module.css"

const GreenhouseCart = ({greenhouse, handleClick}) => {
  return (
    <div className={styles.GreenhouseCart__Main}
      onClick={handleClick}>
      <p className={styles.GreenhouseCart__Name}>{greenhouse.nameGreenHouse}</p>
      <div className={styles.GreenhouseCart__Info}>
        <p className={styles.GreenhouseCart__Info_Area}>
          Area: {greenhouse.area}
        </p>
        <p className={styles.GreenhouseCart__Info_location}>
          Location: {greenhouse.location}
        </p>
      </div>
      <p className={styles.GreenhouseCart__CropName}>Crop: {greenhouse.cropName}</p>
    </div>
  )
}

export default GreenhouseCart;