import { useEffect, useState } from "react";
import styles from "./ConfirmUsers.module.css";
import PopupModal from "../PopupModal/PopupModal";
import { useAuth } from "../Auth/AuthContext";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import ConfirmUserCart from "../ConfirmUserCart/ConfirmUserCart";

const ConfirmUsers = () => {
  const [ unConfirmedUsers, setUnConfirmedUsers ] = useState([]);
  
  const [ modalIsActive, setModalIsActive ] = useState(false);
  const [ errorMessage, setErrorMessage ] = useState("");

  const auth = useAuth();
  const navigate = useNavigate();

  const fetchUnConfirmedUsers = async (signal) => {
    try
    {
      const response = await axios.get("https://localhost:5102/api/Authorize/GetUnConfirmedUsers", {
        headers: {
          "Content-Type": "application/json"
        },
        withCredentials: true,
        signal: signal
      });

      setUnConfirmedUsers([...response.data]);

      console.log(response);
    }
    catch(error)
    {
      if(axios.isCancel(error))
      {
        console.log("Canceled");
      }
      else if(error.response.status == 401)
      {
        auth.logout();
        navigate("/login");
      }
    }
  }

  useEffect(() => {
    const abortController = new AbortController();

    const executeFetchUnConfirmedUsers = async () => {
      await fetchUnConfirmedUsers(abortController.signal);
    }

    executeFetchUnConfirmedUsers();

    return () => abortController.abort();
  }, []);

  const handleConfirmUser = async (userEmail) => {
    try
    {
      await axios.patch("https://localhost:5102/api/Authorize/ActiveUser", {
        userEmails: [userEmail]
      }, {
        headers: {
          "Content-Type": "application/json"
        },
        withCredentials: true
      });

      setUnConfirmedUsers(unConfirmedUsers.filter(user => user.email != userEmail));
    }
    catch(error)
    {
      if(error.response.status == 401 || 
        error.response.status == 409)
      {
        auth.logout();
        navigate("/login");
      }
      else if(error.response.status == 400)
      {
        setModalIsActive(true);
        setErrorMessage(error.response.data.detail);

        setTimeout(() => {
          setModalIsActive(false);
          setErrorMessage("");
        }, 3000);
      }
    }
  }

  return (
    <div className={styles.ConfirmUsers__Main}>
      <PopupModal isActive={modalIsActive} setIsActive={setModalIsActive}>
        <div className={styles.ConfirmUsers__ModalContent}>
          <p>{errorMessage}</p>
        </div>
      </PopupModal>
      <div className={styles.ConfirmUsers__Wrapper}>
        <div className={styles.ConfirmUsers__Users}>
          {
            unConfirmedUsers.map(user => 
              <ConfirmUserCart user={user} handleConfirm={handleConfirmUser}/>
            )
          }
        </div>
      </div>
    </div>
  )
}

export default ConfirmUsers;