import { useEffect, useState } from "react";
import styles from "./Combobox.module.css"

const Combobox = ({setSelectedItem,
        clearError,
        selectedItem = "", items = [],
        errorMessage = ""}) => {
  const [ listItemIsOpen, setListItemIsOpen ] = useState(false);
  
  const handleSelectItem = (item) => {
    setSelectedItem(item);
  }

  const [ errorMessageClass, setErrorMessageClass] = useState("");
  
    useEffect(() => {
      if(errorMessage == "")
      {
        setErrorMessageClass(`${styles.Combobox__ErrorMessage} ${styles.Combobox__ErrorMessageHiden}`);
      }
      else
      {
        setErrorMessageClass(`${styles.Combobox__ErrorMessage}`);
  
        setTimeout(() => {
          setErrorMessageClass(`${styles.Combobox__ErrorMessage} ${styles.Combobox__ErrorMessageHiden}`);
          clearError();
        }, 3000);
      }
    }, [errorMessage])

  return (
    <div className={styles.Combobox__Main}
      onClick={() => setListItemIsOpen(!listItemIsOpen)}>
      <div className={styles.Combobox__SelectedItem}>
        <p className={styles.Combobox__ItemContent}>
          {selectedItem ? selectedItem : "Select Item"}
        </p>
        <div className={styles.Combobox__SelectedArrow}>
          <svg xmlns="http://www.w3.org/2000/svg" xmlnsXlink="http://www.w3.org/1999/xlink" height="30px" width="30px" version="1.1" id="Layer_1" viewBox="0 0 330 330" xml:space="preserve">
            <path id="XMLID_225_" d="M325.607,79.393c-5.857-5.857-15.355-5.858-21.213,0.001l-139.39,139.393L25.607,79.393  c-5.857-5.857-15.355-5.858-21.213,0.001c-5.858,5.858-5.858,15.355,0,21.213l150.004,150c2.813,2.813,6.628,4.393,10.606,4.393  s7.794-1.581,10.606-4.394l149.996-150C331.465,94.749,331.465,85.251,325.607,79.393z"/>
          </svg>
        </div>
      </div>
      <div className={`${styles.Combobox__Items} ${listItemIsOpen ? "" :  styles.Combobox__Items_Hide}`}>
        {
          items &&
          items.map(item => 
            <div className={styles.Combobox__Item}
              key={item}
              onClick={() => {handleSelectItem(item);}}>
              {item}
            </div>
          )
        }
      </div>
      <div className={errorMessageClass}>
        {errorMessage}
      </div>
    </div>
  )
}

export default Combobox;