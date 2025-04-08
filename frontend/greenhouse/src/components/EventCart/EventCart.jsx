import { useCallback, useEffect, useMemo, useState } from "react";
import useDateTimeFormat from "../../services/useDateTimeFormat";
import useTranslateEventStatus from "../../services/useTranslateEventStatus";
import EmptyButton from "../buttons/EmptyButton/EmptyButton";
import styles from "./EventCart.module.css";
import axios from "axios";
import EventResources from "../EventResources/EventResources";
import { useAuth } from "../Auth/AuthContext";
import { useNavigate } from "react-router-dom";
import PrimaryButton from "../buttons/PrimaryButton/PrimaryButton";

const EventCart = ({event, 
  handleProcessEvent, handleCompleteEvent, 
  handleCancelEvent,
  isReadOnly=false}) => {
  const auth = useAuth();
  const navigate = useNavigate();
  const [ eventResources, setEventResources ] = useState([]);
  
  const [ showResources, setShowResource] = useState(false);
  const eventActionIcon = useMemo(() => {
    switch (event.eventStatus) {
      case 0:
        return startIcon;
      case 1:
        return stopIcon;
      case 2:
        return completeIcon;

    }
  }, [event.eventStatus]); 


  const actionBtn = async () => {
    switch (event.eventStatus) {
      case 0:
        await handleProcessEvent(event);
        break;
      case 1:
        await handleCompleteEvent(event.id, eventResources);
        break;
      default:
        alert("Event Completed");
        break;
      }
  }

  const fetchEventResources = async (eventId, signal) => {
    try
    {
      const response = await axios.get(`https://localhost:5202/api/EventResource?eventId=${eventId}`, {
        headers: {
          "Content-Type": "application/json"
        },
        signal: signal
      });

      console.log(response);
      setEventResources([...response.data]);
    }
    catch(error)
    {
      if(axios.isCancel(error))
      {
        console.log("Canceled");
      }
      else if(error.response.status == 404)
      {
        setEventResources([]);
      }
    }
  }

  useEffect(() => {
    const abortController = new AbortController();

    const executeGetResources = async () => {
      await fetchEventResources(event.id, abortController.signal);
    }

    executeGetResources();

    return () => abortController.abort();
  }, [event.id]);

  const handleAddResource = async (resource) => {
    try
    {
      const response = await axios.post("https://localhost:5202/api/EventResource/AddEventResource", {
        eventId: event.id,
        plannedAmount: resource.plannedAmount,
        name: resource.name,
        unit: resource.unit,
        resourceType: resource.resourceType,
      }, {
        headers: {
          "Content-Type": "application/json"
        },
        withCredentials: true
      });

      console.log(response);
      setEventResources(prev => [...prev, {...response.data}]);
    }
    catch(error) 
    { 
      if(error.response.status == 400)
      {
        console.log(error.response.detail);
      }
      else if(error.response.status == 401 ||
        error.response.status == 401
      )
      {
        auth.logout();
        navigate("/login");
      }
    }
  }

  const handleUpdateCountResource = (resourceToUpdate) => {
    setEventResources(eventResources.map(resource =>
      resource.id == resourceToUpdate.id ? resourceToUpdate : resource
    ));
  }

  return (
    <div className={styles.EventCart__Main}>
      <div className={styles.EventCart__Event}>
        <p className={styles.EventCart__Name}>
          {event.name}
        </p>
        <p className={styles.EventCart__Status}>
          {useTranslateEventStatus(event.eventStatus)}
        </p>
        <p className={styles.EventCart__PlannedDate}>
          {useDateTimeFormat(event.plannedDate)}
        </p>
        <p className={styles.EventCart__ActualDate}>
          {event.actualDate ? useDateTimeFormat(event.actualDate) : "Not set"}
        </p>
        <div className={styles.EventCart__Action}>
          <EmptyButton action={isReadOnly ? () => {} : actionBtn}>
            <div className={styles.EventCart__Action_Btn}>
              {eventActionIcon}
            </div>
          </EmptyButton>
        </div>
        {
          isReadOnly == false && 
          <div className={styles.EventCart__Actions}>
            {
              event.eventStatus == 0 &&
              <div className={styles.EventCart__CancelEvent}>
                <div className={styles.EventCart__CancelEventBtn}>
                  <PrimaryButton text={"✖"}
                    action={() => handleCancelEvent(event.id)}
                    backgroundColor={"#d43d65"}
                    color={"#fff"}
                    hoverBackgroundColor={"#000"}
                    hoverColor={"#fff"}/>
                </div>
              </div>
            }
            <div className={styles.EventCart__ShowResource}>
              <div className={styles.EventCart__ShowBtnWrapper}>
                <EmptyButton action={() => setShowResource(!showResources)}>
                  <svg xmlns="http://www.w3.org/2000/svg" xmlnsXlink="http://www.w3.org/1999/xlink" height="30px" width="30px" version="1.1" id="Layer_1" viewBox="0 0 330 330" xmlSpace="preserve">
                    <path id="XMLID_225_" d="M325.607,79.393c-5.857-5.857-15.355-5.858-21.213,0.001l-139.39,139.393L25.607,79.393  c-5.857-5.857-15.355-5.858-21.213,0.001c-5.858,5.858-5.858,15.355,0,21.213l150.004,150c2.813,2.813,6.628,4.393,10.606,4.393  s7.794-1.581,10.606-4.394l149.996-150C331.465,94.749,331.465,85.251,325.607,79.393z"/>
                  </svg>
                </EmptyButton>
              </div>
            </div>
          </div>
        }
      </div>
      {
        isReadOnly == false &&

        <div className={`${styles.EventCart__EventResources} ${showResources ? styles.EventCart__EventResourcesTrasitionShow : `${styles.EventCart__HideResources} ${styles.EventCart__EventResourcesTrasitionHide}` }`}>
          <EventResources 
            addResource={handleAddResource}
            updateResource={handleUpdateCountResource} 
            event={event} resources={eventResources}/>
        </div>
      }
    </div>
  )
}

export default EventCart;

const startIcon = 
  <svg xmlns="http://www.w3.org/2000/svg" xmlnsXlink="http://www.w3.org/1999/xlink" height="30px" width="30px" version="1.1" id="Capa_1" viewBox="0 0 155.908 155.908" xmlSpace="preserve">
    <g>
      <path d="M143.878,58.015L46.02,6.45c-9.816-5.172-21.348-4.838-30.848,0.894C5.672,13.076,0,23.122,0,34.218v87.473   c0,11.096,5.672,21.142,15.172,26.874c5.018,3.028,10.601,4.549,16.2,4.549c5.001,0,10.016-1.215,14.647-3.655l97.858-51.566   c7.42-3.91,12.03-11.55,12.03-19.938S151.298,61.926,143.878,58.015z M135.486,81.968l-97.858,51.566   c-4.249,2.239-9.045,2.101-13.157-0.381C20.358,130.671,18,126.493,18,121.69V34.218c0-4.803,2.358-8.981,6.471-11.462   c2.169-1.309,4.529-1.966,6.898-1.966c2.122,0,4.251,0.527,6.259,1.585l97.858,51.565c2.186,1.152,2.422,3.191,2.422,4.014   S137.672,80.816,135.486,81.968z"/>
    </g>
  </svg>

const stopIcon = 
  <svg xmlns="http://www.w3.org/2000/svg" xmlnsXlink="http://www.w3.org/1999/xlink" fill="#000000" height="30px" width="30px" version="1.1" id="Layer_1" viewBox="0 0 512 512" xmlSpace="preserve">
    <g>
    	<g>
    		<path d="M501.333,96H10.667C4.779,96,0,100.779,0,106.667v298.667C0,411.221,4.779,416,10.667,416h490.667    c5.888,0,10.667-4.779,10.667-10.667V106.667C512,100.779,507.221,96,501.333,96z M490.667,394.667H21.333V117.333h469.333    V394.667z"/>
    	</g>
    </g>
  </svg>

const completeIcon = 
  <svg xmlns="http://www.w3.org/2000/svg" fill="#000000" width="30px" height="30px" viewBox="0 0 24 24" id="d9090658-f907-4d85-8bc1-743b70378e93" data-name="Livello 1"><title>prime</title><path id="70fa6808-131f-4233-9c3a-fc089fd0c1c4" data-name="done circle" d="M12,0A12,12,0,1,0,24,12,12,12,0,0,0,12,0ZM11.52,17L6,12.79l1.83-2.37L11.14,13l4.51-5.08,2.24,2Z"/></svg>
