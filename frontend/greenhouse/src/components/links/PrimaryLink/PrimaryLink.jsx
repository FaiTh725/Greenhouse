import { useNavigate } from "react-router-dom"
import styles from "./PrimaryLink.module.css"

const PrimaryLink = ({direction, text}) => {
  const navigate = useNavigate();
  
  return (
    <p className={styles.PrimaryLink__Main} 
      onClick={() => navigate(direction)}>
      {text}
    </p>
  )
}

export default PrimaryLink