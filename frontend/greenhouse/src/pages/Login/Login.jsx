import { useState } from "react";
import PrimaryButton from "../../components/buttons/PrimaryButton/PrimaryButton";
import PrimaryInput from "../../components/inputs/PrimaryInput/PrimaryInput";
import PrimaryLink from "../../components/links/PrimaryLink/PrimaryLink";
import styles from "./Login.module.css";
import { useAuth } from "../../components/Auth/AuthContext";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const Login = () => {
  const auth = useAuth();
  const navigate = useNavigate();
  const [ form, setForm ] = useState({
    email: "",
    password: ""
  });
  const [ formErrors, setFormErrors ] = useState({
    emailError: "",
    passwordError: ""
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
      nameError: "",
      emailError: "",
      passwordError: "",
      confirmPasswordError: ""
    });
  }

  const handleSendForm = async (e) => {
    e.preventDefault();

    try
    {
      const response = await axios.get(
        `https://localhost:5102/api/Authorize/Login?Email=${form.email}&Password=${form.password}`, {
        headers: {
          "Content-Type" : "application/json"
        },
        withCredentials: true
      });

      auth.login({...response.data});
      navigate("/");
    }
    catch(error)
    {
      if(error.response.status == 400)
      {
        setFormErrors(prev => ({
          ...prev, 
          emailError: error.response.data.detail
        }));
      }
      else if(error.response.status == 404)
      {
        setFormErrors(prev => ({
          ...prev, 
          passwordError: error.response.data.detail
        }));
      }
    }
  }

  return (
    <div className={styles.Login__Main}>
      <div className={styles.Login__Wrapper}>
        <p className={styles.Login__PageName}>Log In</p>
        <h1 className={styles.Login__HeaderText}>Enter your credentials</h1>
        <div className={styles.Login__Form}>
          <h2 className={styles.Login__From_Name}>Log In</h2>
          <div className={styles.Login__Form_InputWrapper}>
            <PrimaryInput setValue={handleChangeForm} value={form.email} 
                          inputName="email" placeholder="Email Address"
                          errorMessage={formErrors.emailError}
                          clearError={handleClearForm}/>
          </div>
          <div className={styles.Login__Form_InputWrapper}>
            <PrimaryInput setValue={handleChangeForm} value={form.password} 
                          inputName="password" placeholder="Password"
                          inputType="password" errorMessage={formErrors.passwordError}
                          clearError={handleClearForm}/>
          </div>
        </div>
        <div className={styles.Login__BtnForm}>
          <PrimaryButton text={"Log In"} action={handleSendForm}
            backgroundColor={"#2fee9e"}
            color={"#fff"}
            hoverBackgroundColor={"#000"}
            hoverColor={"#fff"}/>
        </div>
        <div className={styles.Login__Links}>
          <div className={styles.Login__Links_SignIn}>
            <p>Dont have an account?</p>
            <PrimaryLink text={"Create an account"} direction={"/signin"}/>
          </div>
        </div>
      </div>
    </div>
  )
}

export default Login;