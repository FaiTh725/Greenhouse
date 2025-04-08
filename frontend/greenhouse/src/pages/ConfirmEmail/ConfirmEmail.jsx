import { useState } from "react";
import PrimaryButton from "../../components/buttons/PrimaryButton/PrimaryButton";
import PrimaryInput from "../../components/inputs/PrimaryInput/PrimaryInput";
import PrimaryLink from "../../components/links/PrimaryLink/PrimaryLink";
import styles from "./ConfirmEmail.module.css"
import { useNavigate } from "react-router-dom";
import axios from "axios";

const ConfirmEmail = () => {
  const [ confirmRequestSended, setConfirmRequestSended ] = useState(false);
  const navigate = useNavigate();
  const [ form, setForm ] = useState({
    email: "",
    code: ""
  });
  const [ formErrors, setFormErrors ] = useState({
    emailError: "",
    codeError: ""
  });

  const handleChangeForm = (e) => {
    const key = e.target.name;
    const newValue = e.target.value;

    setForm(prev => ({
      ...prev,
      [key] : newValue
    }));
  }

  const handleClearForm = () => {
    setFormErrors({
      emailError: "",
      codeError: ""
    });
  }

  const handleSendConfrimEmailForm = async (e) => {
    e.preventDefault();

    if(!/^[^@\s]+@[^@\s]+\.[^@\s]+$/.exec(form.email))
    {
      setFormErrors(prev => ({
        ...prev,
        emailError: "Incorrect email"
      }));
      return;
    }

    try
    {
      await axios.post("https://localhost:5102/api/Authorize/ConfirmEmail", {
        email: form.email
      }, {
        headers: {
          "Content-Type": "application/json"
        }
      });

      setConfirmRequestSended(true);
    }
    catch(error)
    {
      if(error.response.status == 409)
      {
        setConfirmRequestSended(true);

        setFormErrors(prev => ({
          ...prev, 
          emailError: error.response.data.detail
        }));
      }
    }
  }

  const handleSendCodeHandler = async (e) => {
    e.preventDefault();

    if(!/^[^@\s]+@[^@\s]+\.[^@\s]+$/.exec(form.email))
    {
      setFormErrors(prev => ({
        ...prev,
        emailError: "Incorrect email"
      }));
      return;
    }

    try
    {
      await axios.post("https://localhost:5102/api/Authorize/AcceptEmailConfirmation", {
        userEmail: form.email,
        code: form.code
      }, {
        headers: {
          "Content-Type": "application/json"
        }
      });

      localStorage.setItem("user_email", form.email);
      navigate("/signin");
    }
    catch(error)
    {
      if(error.response.status == 404)
      {
        setFormErrors(prev => ({
          ...prev,
          codeError: error.response.data.detail
        }))
      }
    }
  }

  return (
    <div className={styles.ConfirmEmail__Main}>
      <div className={styles.ConfirmEmail__Wrapper}>
        <div className={styles.ConfirmEmail__HeaderSection}>
          <h1 className={styles.ConfirmEmail__HeaderText}>
            To register you should have real Email
          </h1>
          <p className={styles.ConfirmEmail__Description}>
            Using Email we'll send you notification <br/> 
            to interact with cite
          </p>
        </div>
        <div className={styles.ConfirmEmail__Body}>
          <p className={styles.ConfirmEmail__PageName}>Confirm</p>
          <div className={styles.ConfirmEmail__Form}>
            <PrimaryInput placeholder="Email" 
              value={form.email} setValue={handleChangeForm}
              clearError={handleClearForm} errorMessage={formErrors.emailError}
              inputName="email"/>
            <div className={`${styles.ConfirmEmail__ConfirmCode} ${confirmRequestSended ? "" : styles.ConfirmEmail__ConfirmCodeHide}`}>
              <PrimaryInput placeholder="Code" 
                value={form.code} setValue={handleChangeForm}
                clearError={handleClearForm} errorMessage={formErrors.codeError}
                inputName="code"/>
            </div>
          </div>
          <div className={styles.ConfirmEmail__BtnWrapper}>
            <PrimaryButton text={"Confirm"} 
              action={confirmRequestSended ? handleSendCodeHandler : handleSendConfrimEmailForm}
              backgroundColor={"#2fee9e"}
              color={"#fff"}
              hoverBackgroundColor={"#000"}
              hoverColor={"#fff"}/>
          </div>
          <div className={styles.ConfirmEmail__Links}>
            <div className={styles.ConfirmEmail__Links_Link}>
              <p>You've already registered?</p>
              <PrimaryLink text={"Log In"} direction={"/login"}/>
            </div>
          </div>
          <div className={`${styles.ConfirmEmail__Repeat} ${confirmRequestSended ? "" : styles.ConfirmEmail__RepeatHiden}`}>
            <p>Repeat sent code</p>

            <PrimaryButton text={"Repeat"} action={() => {setConfirmRequestSended(false); setForm({email: "", code: ""})}}
              backgroundColor={"#2fee9e"}
              color={"#fff"}
              hoverBackgroundColor={"#000"}
              hoverColor={"#fff"}/> 
          </div>
        </div>
      </div>
    </div>
  )
}

export default ConfirmEmail;