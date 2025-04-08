import { useEffect, useState } from "react";
import styles from "./PrimaryInput.module.css"

const PrimaryInput = ({
  value, setValue, clearError,
  inputType = "text", inputName = "",
  placeholder = "", errorMessage = ""}) => {

  const [ errorMessageClass, setErrorMessageClass] = useState("");

  useEffect(() => {
    if(errorMessage == "")
    {
      setErrorMessageClass(`${styles.PrimaryInput__ErrorMessage} ${styles.PrimaryInput__ErrorMessageHiden}`);
    }
    else
    {
      setErrorMessageClass(`${styles.PrimaryInput__ErrorMessage}`);

      setTimeout(() => {
        setErrorMessageClass(`${styles.PrimaryInput__ErrorMessage} ${styles.PrimaryInput__ErrorMessageHiden}`);
        clearError();
      }, 3000);
    }
  }, [errorMessage])

  return (
    <div className={styles.PrimaryInput__Main}>
      <input className={styles.PrimaryInput__Input} 
        value={value} type={inputType} 
        name={inputName} onChange={setValue}
        placeholder={placeholder}/>
      <div className={errorMessageClass}>
        {errorMessage}
      </div>
    </div>
  )
}

export default PrimaryInput;