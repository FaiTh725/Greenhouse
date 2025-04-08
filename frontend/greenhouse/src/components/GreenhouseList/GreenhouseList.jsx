import GreenhouseCart from "../GreenhouseCart/GreenhouseCart";
import styles from "./GreenhouseList.module.css"

const GreenhouseList = ({greenhouses = [], selectGreenhouse}) => {
  return (
    <div className={styles.GreenhouseList__Main}>
      <div className={styles.GreenhouseList__Wrapper}>
        {greenhouses && 
        greenhouses.map(greenhouse => 
          <GreenhouseCart key={greenhouse.id} greenhouse={greenhouse}
            handleClick={() => selectGreenhouse({...greenhouse})}/>
        )}
      </div>
    </div>
  )
}

export default GreenhouseList;