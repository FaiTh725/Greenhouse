import { useEffect, useState } from "react";
import PrimaryButton from "../buttons/PrimaryButton/PrimaryButton";
import Combobox from "../inputs/Combobox/Combobox";
import PrimaryInput from "../inputs/PrimaryInput/PrimaryInput";
import styles from "./AddEvent.module.css";
import axios from "axios";

const AddEvent = ({addEvent, closeForm}) => {
  const [ form, setForm ] = useState({
    name: "",
    minutes: "",
    hours: ""
  }); 
  const [ errorForm, setErrorForm ] = useState({
    nameError: "",
    minutesError: "",
    hoursError: "",
    eventTypeError: "",
    employeIdError: ""
  });
  const [ avaliableEmployes, setAvaliableEmployes ] = useState([]);
  const [ selectedEmploye, setSelectedEmploye ] = useState(null);

  const eventTypes = [
    {
      type: 0,
      name: "Seeding"
    },
    {
      type: 1,
      name: "Watering"
    },
    {
      type: 2,
      name: "Fertilizing"
    },
    {
      type: 3,
      name: "Harvesting"
    },
  ];
  const [ selectedEvent, setSelectedEvent ] = useState(null);

  const handleChangeForm = (e) => {
    const key = e.target.name;
    const newValue = e.target.value;

    setForm(prev => ({
      ...prev,
      [key]: newValue
    }));
  }

  const handleClearErrorForm = () => {
    setErrorForm({
      nameError: "",
      minutesError: "",
      hoursError: ""
    });
  }

  const handleFetchEmployes = async (signal) => {
    try
    {
      const response = await axios.get("https://localhost:5202/api/Employee/GetEmployes", {
        headers: {
          "Content-Type": "application/json"
        },
        signal: signal
      });

      setAvaliableEmployes([...response.data]);
    }
    catch(error)
    {
      if(axios.isCancel(error))
      {
        console.log("Canceled");
      }
      else
      {
        console.error(error.message);
        setAvaliableEmployes([]);
      }
    }
  }

  useEffect(() => {
    const abortSignal = new AbortController();
    const executeGetEmplyes = async () => {
      await handleFetchEmployes(abortSignal.signal);
    }

    executeGetEmplyes();

    return () => abortSignal.abort();
  }, []);

  const handleAddEvent = async (e) => {
    e.preventDefault();

    var formIsValid = true;

    if(!form.name)
    {
      setErrorForm(prev => ({
        ...prev,
        nameError: "Name is required"
      }));
      formIsValid = false;
    }
    const hours = Number(form.hours);
    if(!form.hours || isNaN(hours) ||
      hours > 12 || hours < 0)
    {
      setErrorForm(prev => ({
        ...prev,
        hoursError: "Hours should be Number and hours value"
      }));
      formIsValid = false;
    }
    const minutes = Number(form.minutes);
    if( !form.minutes || isNaN(minutes) ||
      minutes > 59 || minutes < 0)
    {
      setErrorForm(prev => ({
        ...prev,
        minutesError: "minutes should be Number and minutes value"
      }));
      formIsValid = false;
    }
    if(!selectedEmploye)
    {
      setErrorForm(prev => ({
        ...prev,
        employeIdError: "Select executer emplyer"
      }));
      formIsValid = false;
    }
    if(!selectedEvent)
    {
      setErrorForm(prev => ({
        ...prev,
        eventTypeError: "Select event type"
      }));
      formIsValid = false;
    }

    
    if(!formIsValid)
    {
      return;
    }

    const eventTypeNumber = eventTypes.find(eventType => eventType.name == selectedEvent).type;
    const employeId = avaliableEmployes.find(employe => employe.email == selectedEmploye).id;

    await addEvent({
      name: form.name,
      hours: hours,
      minutes: minutes,
      eventType: eventTypeNumber,
      employeId: employeId 
    });

    closeForm();
  }

  return (
    <div className={styles.AddEvent__Main}>
      <div className={styles.AddEvent__Wrapper}>
        <div className={styles.AddEvent__Form}>
          <div className={styles.AddEvent__FormInput}>
            <PrimaryInput placeholder="Name" 
              value={form.name}
              inputName="name"
              setValue={handleChangeForm}
              errorMessage={errorForm.nameError}
              clearError={handleClearErrorForm}/>
          </div>
          <div className={styles.AddEvent__FormInput}>
            <PrimaryInput placeholder="Hours" 
              value={form.hours}
              inputName="hours"
              setValue={handleChangeForm}
              errorMessage={errorForm.hoursError}
              clearError={handleClearErrorForm}/>
          </div>
          <div className={styles.AddEvent__FormInput}>
            <PrimaryInput placeholder="Minutes" 
              value={form.minutes}
              inputName="minutes"
              setValue={handleChangeForm}
              errorMessage={errorForm.minutesError}
              clearError={handleClearErrorForm}/>
          </div>
          <div className={styles.AddEvent__FormInput}>
            <Combobox items={eventTypes.map(event => event.name)}
                      setSelectedItem={setSelectedEvent}
                      selectedItem={selectedEvent}
                      errorMessage={errorForm.eventTypeError}
                      clearError={handleClearErrorForm}/>
          </div>
          <div className={styles.AddEvent__FormInput}>
            <Combobox selectedItem={selectedEmploye} 
                  items={avaliableEmployes.map(employe => employe.email)}
                  setSelectedItem={setSelectedEmploye}
                  errorMessage={errorForm.employeIdError}
                  clearError={handleClearErrorForm}/>
          </div>
        </div>
        <div className={styles.AddEvent__SendFormBtn}>
          <PrimaryButton 
            action={handleAddEvent}
            text={"Create"}
            backgroundColor={"#2fee9e"}
            color={"#fff"}
            hoverBackgroundColor={"#000"}
            hoverColor={"#fff"}/>  
        </div>
      </div>
    </div>
  )
}

export default AddEvent;