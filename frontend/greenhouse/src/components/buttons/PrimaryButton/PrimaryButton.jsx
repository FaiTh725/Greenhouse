import styles from "./PrimaryButton.module.css";

const PrimaryButton = ({text, action, backgroundColor, color, 
    hoverBackgroundColor, hoverColor}) => {
  return (
    <div className={styles.PrimaryButton__Main} onClick={action}
        style={{
          '--bg-color': backgroundColor,
          '--hover-bg-color': hoverBackgroundColor,
          '--text-color': color,
          '--hover-color': hoverColor
        }}>
      <p className={styles.PrimaryButton__Content}>
        {text}
      </p>
    </div>
  )
}

export default PrimaryButton;