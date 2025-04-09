import { useEffect, useState } from "react";
import Header from "../../components/Header/Header";
import styles from "./Profile.module.css"
import axios from "axios";
import EventCart from "../../components/EventCart/EventCart";
import { useAuth } from "../../components/Auth/AuthContext";
import ConfirmUsers from "../../components/ConfirmUsers/ConfirmUsers";

const Profile = () => {
  const [ employeEvents, setEmployeEvents ] = useState([]);
  
  const auth = useAuth();

  const handleFetchEmployeEvents = async (signal) => {
    try
    { 
      const response = await axios.get("https://localhost:5202/api/Employee/GetEmployeEvents", {
        headers: {
          "Content-Type": "application/json"
        },
        withCredentials: true,
        signal: signal
      });

      setEmployeEvents([...response.data]);
      console.log(response);
    }
    catch(error)
    {
      if(axios.isCancel(error))
      {
        console.log("canceled");
      }
      else if(error.response.status == 400)
      {
        console.log(error.response.data.detail);
      }
    }
  }

  useEffect(() => {
    const abortController = new AbortController();

    const executeGetEmployeEvents = async () => {
      await handleFetchEmployeEvents(abortController.signal);
    }

    executeGetEmployeEvents();

    return () => abortController.abort();
  }, []);


  return (
    <div className={styles.Profile__Main}>
      <Header/>
      <div className={styles.Profile__Welcome}>
        <p>Here is you progress</p>
      </div>
      <div className={styles.Profile__AdminConfrimUsers}>
        {
          auth.user && auth.user.role == "Admin" &&
          <ConfirmUsers/>
        }
      </div>
      <div className={styles.Profile__Events}>
        <div className={`${styles.Profile__CompletedEvents} ${styles.Profile__EventSection}`}>
          <p className={styles.Profile__EventSectionHeader}>Complited Events</p>
          <div className={styles.Profile__EventsContent}>
            {
              employeEvents
              .filter(event => event.eventStatus == 2)
              .map(event => 
                <EventCart event={event} isReadOnly={true}/>
              )
            }
          </div>
        </div>
        <div className={`${styles.Profile__UncompltedEvents} ${styles.Profile__EventSection}`}>
          <p className={styles.Profile__EventSectionHeader}>UnCompleted Events</p>
          <div className={styles.Profile__EventsContent}>
            {
              employeEvents
              .filter(event => event.eventStatus == 1)
              .map(event => 
                <EventCart event={event} isReadOnly={true}/>
              )
            }
          </div>
        </div>
        <div className={`${styles.Profile__PlannedEvents} ${styles.Profile__EventSection}`}>
          <p className={styles.Profile__EventSectionHeader}>Planned Events</p>
          <div className={styles.Profile__EventsContent}>
            {
              employeEvents
              .filter(event => event.eventStatus == 0)
              .map(event => 
                <EventCart event={event} isReadOnly={true}/>
              )
            }
          </div>
        </div>
      </div>
    </div>
  )
}

export default Profile;