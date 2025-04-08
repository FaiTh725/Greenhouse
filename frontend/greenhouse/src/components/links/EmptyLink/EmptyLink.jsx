import { useNavigate } from "react-router-dom";
import styles from "./EmptyLink.module.css"

const EmptyLink = ({direction, children}) => {
  const navigate = useNavigate();
  
  return (
    <div className={styles.EmptyLink__Main} 
        onClick={() => {navigate(direction)}}>
      <div className={styles.EmptyLink__Content}>
        {children}
      </div>
    </div>
  )
}

export default EmptyLink;