import { useNavigate } from "react-router-dom";
import { useAuth } from "../../components/Auth/AuthContext";
import PrimaryButton from "../../components/buttons/PrimaryButton/PrimaryButton";
import PrimaryInput from "../../components/inputs/PrimaryInput/PrimaryInput";
import PrimaryLink from "../../components/links/PrimaryLink/PrimaryLink";
import styles from "./Signin.module.css";
import { useEffect, useState } from "react";
import axios from "axios";
import PopupModal from "../../components/PopupModal/PopupModal";

const Signin = () => {
  const auth = useAuth();
  const navigate = useNavigate();
  const [ form, setForm ] = useState({
    name: "",
    email: "",
    password: "",
    confirmPassword: ""
  });
  const [ formErrors, setFormErrors ] = useState({
    nameError: "",
    emailError: "",
    passwordError: "",
    confirmPasswordError: ""
  });
  const [ message, setMessage ] = useState("");
  const [ modalIsOpen, setModalIsOpen ] = useState(false);

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

    var isValidForm = true;

    if(form.password != form.confirmPassword)
    {
      setFormErrors(prev => ({
        ...prev,
        confirmPasswordError: "Passwords are difference"
      }));
      isValidForm = false;
    }
    if(!/^(?=.*[A-Za-z])(?=.*\d).+$/.exec(form.password))
    {
      setFormErrors(prev => ({
        ...prev,
        passwordError: "Password have to contains one letter and 1 number"
      }));
      isValidForm = false;
    }
    if(!/^[^@\s]+@[^@\s]+\.[^@\s]+$/.exec(form.email))
    {
      setFormErrors(prev => ({
        ...prev,
        emailError: "Incorrect email"
      }));
      isValidForm = false;
    }
    if(form.password.length < 5 ||
      form.password.length > 30
    )
    {
      setFormErrors(prev => ({
        ...prev,
        passwordError: "Password have to in range from 5 to 30"
      }));
      isValidForm = false;
    }

    if(!isValidForm)
    {
      return;
    }
    try
    {
      const response = await axios.post(
        "https://localhost:5102/api/Authorize/Register", {
          email: form.email,
          name: form.name,
          password: form.password
        }, {
        headers: {
          "Content-Type" : "application/json"
        },
        withCredentials: true
      });

      setModalIsOpen(true);
      setMessage("Please Wait When Admin Comfirm Your Account");

      setTimeout(() => {
        setModalIsOpen(false);
        setMessage("");
      }, 3000);
    }
    catch(error)
    {
      if(error.response.status == 400 ||
        error.response.status == 409
      )
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

  useEffect(() => {
    const email = localStorage.getItem("user_email");
    if(email)
    {
      setForm(prev => ({
        ...prev,
        email: email
      }));
    }

    localStorage.removeItem("user_email");
  }, []);

  return (
    <div className={styles.Signin__Main}>
      <PopupModal isActive={modalIsOpen} setIsActive={setModalIsOpen}>
        <div className={styles.Signin__Main_ModalContent}>
          <p>{message}</p>
        </div>
      </PopupModal>
      <div className={styles.Signin__Wrapper}>
        <p className={styles.Signin__PageName}>Sign Up</p>
        <h1 className={styles.Signin__HeaderText}>Create new account</h1>
        <div className={styles.Signin__Form}>
          <h2 className={styles.Signin__From_Name}>Create New Account</h2>
          <PrimaryInput placeholder="Name" errorMessage={formErrors.nameError}
                        setValue={handleChangeForm} clearError={handleClearForm}
                        inputName="name"/>
          <PrimaryInput placeholder="Email Address" errorMessage={formErrors.emailError}
                        setValue={handleChangeForm} clearError={handleClearForm}
                        inputName="email"/>
          <PrimaryInput placeholder="Password" inputType="password" 
                        inputName="password" setValue={handleChangeForm} 
                        errorMessage={formErrors.passwordError} clearError={handleClearForm}/>
          <PrimaryInput placeholder="Confirm Password" inputType="password" 
                        inputName="confirmPassword" setValue={handleChangeForm} 
                        errorMessage={formErrors.confirmPasswordError} clearError={handleClearForm}/>
        </div>
        <div className={styles.Signin__BtnForm}>
          <PrimaryButton text={"Create Account"} action={handleSendForm}
            backgroundColor={"#2fee9e"}
            color={"#fff"}
            hoverBackgroundColor={"#000"}
            hoverColor={"#fff"}/>
        </div>
        <div className={styles.Signin__Links}>
          <div className={styles.Signin__Links_SignIn}>
            <p>Already have an accunt?</p>
            <PrimaryLink text={"Log In"} direction={"/login"}/>
          </div>
          <div className={styles.Signin__Links_SignIn}>
            <p>Maybe your email is unconfirmed?</p>
            <PrimaryLink text={"Confirm"} direction={"/confirm"}/>
          </div>
        </div>
      </div>
    </div>
  )
}

export default Signin;