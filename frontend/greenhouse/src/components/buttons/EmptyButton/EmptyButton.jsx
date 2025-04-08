import styles from "./EmptyButton.module.css";

const EmptyButton = ({action, children}) => {
  return (
    <div className={styles.EmptyButton__Main} onClick={action}>
      {children}
    </div>
  )
}

export default EmptyButton;