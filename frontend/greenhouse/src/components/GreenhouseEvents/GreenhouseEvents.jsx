import { useEffect, useState } from "react";
import styles from "./GreenhouseEvents.module.css";
import axios from "axios";
import EventCart from "../EventCart/EventCart";
import DatePicker from "react-date-picker";
import 'react-date-picker/dist/DatePicker.css';
import 'react-calendar/dist/Calendar.css';
import formatDate from "../../services/useDateFormat";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../Auth/AuthContext";
import PopupModal from "../PopupModal/PopupModal";
import PrimaryButton from "../buttons/PrimaryButton/PrimaryButton";
import AddEvent from "../AddEvent/AddEvent";

const GreenhouseEvents = ({greenhouse, triggerMessage}) => {
  const [ events, setEvents ] = useState([]);
  const [ pickedDate, setPickedDate] = useState(new Date().toLocaleDateString("fr-CA").split("T")[0]);
  
  const [ createEventModalIsActive, setCreateEventModalIsActive ] = useState(false);
  
  const navigate = useNavigate();
  const auth = useAuth();

  const handleFetchEvents = async (abortSignal) => {
    const date = new Date(pickedDate).toLocaleDateString("fr-CA").split("T")[0];
    
    if(date == null)
    {
      return;
    }
    
    try
    {
      const response = await axios.get(
        `https://localhost:5202/api/Greenhouse/GetGreenhouseEventsByDay?id=${greenhouse.id}&eventsDay=${date}`, {
        headers: {
          "Content-Type": "application/json"
        },
        signal: abortSignal,
        withCredentials: true
      });

      setEvents([...response.data.events]);
      console.log([...response.data.events]);
    }
    catch (error)
    {
      if(axios.isCancel(error))
      {
        console.log("Canceled");
      }
      else if(error.response.status == 404)
      {
        setEvents([]);
      }
    }
  }

  useEffect(() => {
    const abortController = new AbortController();

    const executeFetchEvents = async () => {
      await handleFetchEvents(abortController.signal);
    }

    executeFetchEvents();

    return () => abortController.abort();
  }, [pickedDate]);

  const handleStartEvent = async (event) => {
    if(event.eventStatus != 0)
    {
      triggerMessage("Invalid Operation");
      return;
    }
    if(event.executingEmail != auth.user.email)
    {
      triggerMessage("Process Event Only Execute - " + event.executingEmail);
      return;
    }
    try
    {
      await axios.patch("https://localhost:5202/api/GreenhouseEvent/ProcessEvent", {
        eventId: event.id
      }, {
        headers: {
          "Content-Type": "application/json"
        },
        withCredentials: true
      });
      const eventId = event.id
      setEvents(prevEvents => prevEvents.map(event => 
        event.id == eventId  ? {...event, eventStatus: 1} : event
      ));
    }
    catch (error)
    {
      console.log(error);
      if(error.response.status == 400)
      {
        triggerMessage(error.message);
      }
      else if(error.response.status == 401 || 
        error.response.status == 409)
      {
        auth.logout();
        navigate("/signin");
      }
      else
      {
        console.error("Something error with process event");
      }
    }
  }

  const handleCompleteEvent = async (eventId, eventResources) => {

    try
    {
      const response = await axios.post("https://localhost:5202/api/GreenhouseEvent/CompleteEvent", {
        eventId: eventId,
        actualResources: eventResources.map(resource => ({
          eventResourceId: resource.id,
          amount: resource.actualAmount ? resource.actualAmount : 0        }))
      }, {
        headers: {
          "Content-Type": "application/json"
        },
        withCredentials: true
      });

      console.log(response);
      // Доделать
      setEvents(prevEvents => prevEvents.map(event => 
        event.id == eventId ? {...event, eventStatus: 2} : event
      ));
    }
    catch (error) 
    {
      if(error.response.status == 401 ||
        error.response.status == 409)
      {
        auth.logout();
        navigate("/login");
      }
      else if(error.response.status == 400 )
      {
        triggerMessage(error.response.data.detail);
      }
    }
  }

  const handleAddEvent = async (addEvent) => {
    const date = new Date(pickedDate);
    date.setHours(addEvent.hours, addEvent.minutes, 0);

    console.log(date);
    try
    {
      const response = await axios.post("https://localhost:5202/api/GreenhouseEvent/AddGeenhouseEvent", {
        name: addEvent.name,
        plannedDate: date,
        eventType: addEvent.eventType,
        employeId: addEvent.employeId,
        greenhouseId: greenhouse.id
      }, {
        headers: {
          "Content-Type": "application/json"
        },
        withCredentials: true
      });

      console.log(response);
      setEvents(prev => [...prev, {...response.data}]);
    }
    catch (error)
    {
      if(error.response.status == 401 ||
        error.response.status == 405)
      {
        auth.logout();
        navigate("login");
      }
      else if(error.response.status == 400)
      {
        triggerMessage(error.message);
      }
    }
  }

  const handleCancelEvent = async (eventId) => {
    try
    {
      const eventIdList = [eventId];

      await axios.delete("https://localhost:5202/api/GreenhouseEvent/CancelGreenhouseEvent", {
        data: {
          idList: eventIdList
        },
        withCredentials: true,
        headers: {
          "Content-Type": "application/json"
        }
      });

      setEvents(events.filter(event => event.id != eventId));
    }
    catch (error)
    {
      console.log(error);
      if(error.response.status == 400)
      {
        triggerMessage("Error Cancel Event");
      }
      else if(error.response.status == 401)
      {
        auth.logout();
        navigate("/login");
      }
    }
  }

  return (
    <div className={styles.GreenhouseEvents__Main}>
      <PopupModal isActive={createEventModalIsActive} setIsActive={setCreateEventModalIsActive}>
        <AddEvent addEvent={handleAddEvent} closeForm={() => setCreateEventModalIsActive(false)}/>
      </PopupModal>
      <div className={styles.GreenhouseEvents__HeaderInfo}>
        <h1 className={styles.GreenhouseEvents__GreenhouseName}>{greenhouse.nameGreenHouse}</h1>
        <p className={styles.GreenhouseEvents__GreenhouseInfo}>Area {greenhouse.area}</p>
        <p className={styles.GreenhouseEvents__GreenhouseInfo}>Location {greenhouse.location}</p>
        <p className={styles.GreenhouseEvents__GreenhouseInfo}>Crop {greenhouse.cropName}</p>
      </div>
      <div className={styles.GreenhouseEvents__Body}>
        <div className={styles.GreenhouseEvents__AddGreenhouse}>
          <div className={styles.GreenhouseEvents__AddBtnWrapper}>
            <PrimaryButton
              action={() => setCreateEventModalIsActive(true)} 
              text={"Create Event"}
              backgroundColor={"#2fee9e"}
              color={"#fff"}
              hoverBackgroundColor={"#000"}
              hoverColor={"#fff"}/>
          </div>
        </div>
        <div className={styles.GreenhouseEvents__Events}>
          <div className={styles.GreenhouseEvents__Events_Header}>
            <p>{pickedDate ? formatDate(pickedDate) : ""}</p>
          </div>
          <div className={styles.GreenhouseEvents__Events_Content}>
            {
              events.map(event => 
                <div key={event.id} className={styles.GreenhouseEvents__Events_EventCart}>
                  <EventCart event={event} 
                  handleCancelEvent={handleCancelEvent}
                    handleCompleteEvent={handleCompleteEvent}
                    handleProcessEvent={handleStartEvent}
                    />
                </div>
              )
            }
          </div>
        </div>
        <div className={styles.GreenhouseEvents__DatePicker}>
          <DatePicker className={styles.GreenhouseEvents__DatePickerComponent} value={pickedDate} onChange={setPickedDate} format="y-MM-dd"/>
        </div>
      </div>
    </div>
  )
}

export default GreenhouseEvents;