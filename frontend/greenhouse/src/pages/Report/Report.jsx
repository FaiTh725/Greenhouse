import { useEffect, useState } from "react";
import PrimaryButton from "../../components/buttons/PrimaryButton/PrimaryButton";
import Header from "../../components/Header/Header";
import Combobox from "../../components/inputs/Combobox/Combobox";
import styles from "./Report.module.css"
import axios from "axios";
import Pagination from "../../components/Pagination/Pagination";
import { useAuth } from "../../components/Auth/AuthContext";
import { useNavigate } from "react-router-dom";
import PopupModal from "../../components/PopupModal/PopupModal";

const Report = () => {
  const [ greenhouses, setGreenhouses ] = useState([]);
  const [ maxGreenhouses, setMaxGreenhouses ]= useState(0);
  const [ events, setEvents ] = useState([]); 
  const [ pagination, setPagination ] = useState({
    page: 1,
    count: 5
  });
  const [ errorModaIsActive, setErrorModalIsActive ] = useState(false);
  const [ errorMessage, setErrorMessage ] = useState("");
  const [ selectedGreenhouse, setSelectedGreenhouse ] = useState(null);
  const [ selectedEvent, setSelectedEvent ] = useState(null);

  const auth = useAuth();
  const navigate = useNavigate();

  const handleFetchGreenhouses = async (signal, page, count) => {
    try
    {
      const response = await axios.get(`https://localhost:5202/api/Greenhouse/GetGreenhouses?page=${page}&count=${count}`, {
        headers: {
          "Content-Type": "application/json"
        },
        signal: signal
      });

      console.log(response);
      setGreenhouses([...response.data.greenhouses]);
      setMaxGreenhouses(response.data.maxCount);
    }
    catch (error)
    {
      if(axios.isCancel(error))
      {
        console.log("Canceled");
      }
      else
      {
        console.error(error.message);
        setGreenhouses([]);
      }
    }
  }

  const handleFetchEvents = async (greenhouseId) => {
    try
    {
      const response = await axios.get(`https://localhost:5202/api/Greenhouse/GetGreenhouseCompletedEvents?id=${greenhouseId}`, {
        headers: {
          "Content-Type": "application/json"
        }
      });

      console.log(response);
      setEvents([...response.data.events]);
    }
    catch (error)
    {
      console.log(error.message);
    }
  }

  useEffect(() => {
    const abortSignal = new AbortController();

    const executeFetchGrenhouses = async () => {
      await handleFetchGreenhouses(abortSignal.signal, pagination.page, pagination.count);
    }

    executeFetchGrenhouses();

    return () => abortSignal.abort();
  }, []);

  useEffect(() => {
    if(selectedGreenhouse == null)
    {
      setEvents([]);
      return;
    }

    const greenhouseId = greenhouses
      .find(greenhouse => greenhouse.nameGreenHouse == selectedGreenhouse).id;

    const executeFetchEvents = async () => {
      await handleFetchEvents(greenhouseId);
    }

    executeFetchEvents();
  }, [selectedGreenhouse]);

  const handleNextGreenhouses = async (e) => {
    e.preventDefault();
  
    const currentCount = (pagination.page - 1) * pagination.count;

    if( currentCount + pagination.count <= maxGreenhouses)   
    {
      setPagination(prev => ({
        ...prev,
        page: pagination.page + 1
      }));

      await handleFetchGreenhouses(
        null,
        pagination.page + 1,
        pagination.count);
    }
  }

  const handlePrevGreenhouses = async (e) => {
    e.preventDefault();

    if(pagination.page == 1)
    {
      return;
    }

    setPagination(prev => ({
      ...prev,
      page: pagination.page - 1
    }));

    await handleFetchGreenhouses(
      null,
      pagination.page - 1,
      pagination.count);
  }

  const triggerMessage = (message) => {
    setErrorModalIsActive(true);
    setErrorMessage(message);

    setTimeout(() => {
      setErrorModalIsActive(false);
      setErrorMessage("");
    }, 3000)
  }

  const handleGetReport = async (e) => {
    e.preventDefault();

    if(selectedEvent == null)
    {
      triggerMessage("Select Any Event");
      return;
    }

    const eventId = events.find(event => event.name == selectedEvent).id;

    try
    {
      const response = await axios.get(`https://localhost:5302/api/Report/GetSpendingResource?eventId=${eventId}`, {
        responseType: "blob",
        withCredentials: true
      });

      const url = window.URL.createObjectURL(new Blob([response.data]));
      const a = document.createElement("a");
      a.href = url;
      a.download = "report.docx";
      document.body.appendChild(a);
      a.click();
      document.body.removeChild(a);
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
        triggerMessage("Some Error");
      }
    }
  }

  return (
    <div className={styles.Report__Main}>
      <Header/>
      <PopupModal 
          isActive={errorModaIsActive} 
          setIsActive={setErrorModalIsActive}>
        <p>{errorMessage}</p>
      </PopupModal>
      <div className={styles.Report__Header}>
        <p className={styles.Report__HeaderText}>Report</p>
      </div>
      <div className={styles.Report__Reports}>
        <div className={styles.Report__GetReport}>
          <p className={styles.Report__ReportName}>Resource Report</p>
          <div className={styles.Report__ReportPagination}>
            <div>
              <p>Greenhouse page {pagination.page}</p>
              <p>Greenhouse count {pagination.count}</p>
            </div>
            <Pagination handleNext={handleNextGreenhouses} handlePrev={handlePrevGreenhouses}/>
          </div>
          <div className={styles.Report__ResourceReport}>
            <div className={styles.Report__ResourceReport__Input}>
              <Combobox items={greenhouses.map(greenhouse => greenhouse.nameGreenHouse)}
                selectedItem={selectedGreenhouse}
                setSelectedItem={setSelectedGreenhouse}/>
            </div>
            <div className={styles.Report__ResourceReport__Input}>
              <Combobox items={events.map(event => event.name)}
                setSelectedItem={setSelectedEvent}
                selectedItem={selectedEvent}/>
            </div>
            <div className={styles.Profile__Reports_GetReportBtn}>
              <PrimaryButton 
                action={handleGetReport}
                text={"Get Report"}
                backgroundColor={"#2fee9e"}
                color={"#fff"}
                hoverBackgroundColor={"#000"}
                hoverColor={"#fff"}/>
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}

export default Report;