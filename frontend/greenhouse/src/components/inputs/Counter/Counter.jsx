import EmptyButton from "../../buttons/EmptyButton/EmptyButton";
import styles from "./Counter.module.css"

const Counter = ({value, 
    setValue, step = 1,
    maxValue = null, minValue = null}) => {
  
  const handleNext = () => {
    if(maxValue != null && value + step > maxValue)
    {
      return;
    }

    setValue(value + 1);
  }

  const handlePrev = () => {
    if(minValue != null && value - step < minValue)
    {
      return;
    }

    setValue(value - 1);
  }

  return (
    <div className={styles.Counter__Main}>
      <div className={styles.Counter__Wrapper}>
        <div className={styles.Counter__PrevBtn}>
          <EmptyButton
            action={handlePrev}>
            <div className={styles.Counter__BtnContent}>
              <svg fill="#5b5c60" height="30px" width="30px" version="1.1" id="Layer_1" xmlns="http://www.w3.org/2000/svg" xmlnsXlink="http://www.w3.org/1999/xlink" viewBox="0 0 330 330" xmlSpace="preserve" transform="matrix(-1, 0, 0, 1, 0, 0)"><g id="SVGRepo_bgCarrier" strokeWidth="0"></g><g id="SVGRepo_tracerCarrier" strokeLinecap="round" strokeLinejoin="round"></g><g id="SVGRepo_iconCarrier"> <path id="XMLID_222_" d="M250.606,154.389l-150-149.996c-5.857-5.858-15.355-5.858-21.213,0.001 c-5.857,5.858-5.857,15.355,0.001,21.213l139.393,139.39L79.393,304.394c-5.857,5.858-5.857,15.355,0.001,21.213 C82.322,328.536,86.161,330,90,330s7.678-1.464,10.607-4.394l149.999-150.004c2.814-2.813,4.394-6.628,4.394-10.606 C255,161.018,253.42,157.202,250.606,154.389z"></path> </g>
              </svg>
            </div>
          </EmptyButton>
        </div>
        <div className={styles.Counter__Value}>
          {value}
        </div>
        <div className={styles.Counter__NextBtn}>
          <EmptyButton
            action={handleNext}>
            <div className={styles.Counter__BtnContent}>
              <svg height="30px" width="30px" version="1.1" id="Layer_1" xmlns="http://www.w3.org/2000/svg" xmlnsXlink="http://www.w3.org/1999/xlink" viewBox="0 0 330 330" xmlSpace="preserve" transform="matrix(1, 0, 0, 1, 0, 0)"><g id="SVGRepo_bgCarrier" strokeWidth="0"></g><g id="SVGRepo_tracerCarrier" strokeLinecap="round" strokeLinejoin="round"></g><g id="SVGRepo_iconCarrier"> <path id="XMLID_222_" d="M250.606,154.389l-150-149.996c-5.857-5.858-15.355-5.858-21.213,0.001 c-5.857,5.858-5.857,15.355,0.001,21.213l139.393,139.39L79.393,304.394c-5.857,5.858-5.857,15.355,0.001,21.213 C82.322,328.536,86.161,330,90,330s7.678-1.464,10.607-4.394l149.999-150.004c2.814-2.813,4.394-6.628,4.394-10.606 C255,161.018,253.42,157.202,250.606,154.389z"></path> </g>
              </svg>
            </div>
          </EmptyButton>
        </div>
      </div>
    </div>
  )
}

export default Counter;