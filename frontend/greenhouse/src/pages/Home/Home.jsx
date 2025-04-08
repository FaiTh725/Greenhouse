import { useEffect, useState } from "react";
import Header from "../../components/Header/Header";
import styles from "./Home.module.css";
import axios from "axios";
import GreenhouseList from "../../components/GreenhouseList/GreenhouseList";
import PopupModal from "../../components/PopupModal/PopupModal";
import AddGreenhouse from "../../components/AddGreenhouse/AddGreenhouse";
import Pagination from "../../components/Pagination/Pagination";
import PrimaryButton from "../../components/buttons/PrimaryButton/PrimaryButton";
import GreenhouseEvents from "../../components/GreenhouseEvents/GreenhouseEvents";

const Home = () => {
  const [ greenhouses, setGreenhouses ] = useState([]);
  const [ maxGreenhouses, setMaxGreenhouses] = useState(0);
  const [ currentGreenhouse, setCurrentGreenhouse ] = useState(null);
  const [ paginationData, setPaginationData ] = useState({
    page: 1,
    count: 5
  }); 
  const [ modalIsActive, setModalIsActive ] = useState(false);
  const [ errorMessage, setErrorMessage] = useState("");
  const [ errorModalIsActive, setErrorModalIsActive] = useState(false);
  const [ addGreenhouse, setAddGreenhouse ] = useState(null);

  const handleFetchGreenhouses = async (abortSignal, page, count) => {
    try
    {
      const response = await axios.get(`https://localhost:5202/api/Greenhouse/GetGreenhouses?page=${page}&count=${count}`, {
        headers: {
          "Content-Type": "application/json"
        },
        signal: abortSignal
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

  useEffect(() => {
    const abortController = new AbortController();

    const excuteGetchGreenhouses = async () => 
      await handleFetchGreenhouses(
        abortController.signal, 
        paginationData.page, 
        paginationData.count);

    excuteGetchGreenhouses();

    return () => abortController.abort();
  }, []);

  useEffect(() => {
    if(addGreenhouse == null)
    {
      return;
    } 

    if(greenhouses.length < paginationData.count)
    {
      setGreenhouses(prev => [...prev, {...addGreenhouse}]);
    }

    setAddGreenhouse(null);
    setModalIsActive(false);
    
  }, [addGreenhouse]);

  const handleNextGreenhouses = async (e) => {
    e.preventDefault();
  
    const currentCount = (paginationData.page - 1) * paginationData.count;

    if( currentCount + paginationData.count <= maxGreenhouses)   
    {
      setPaginationData(prev => ({
        ...prev,
        page: paginationData.page + 1
      }));

      await handleFetchGreenhouses(
        null,
        paginationData.page + 1,
        paginationData.count);
    }
  }

  const handlePrevGreenhouses = async (e) => {
    e.preventDefault();

    if(paginationData.page == 1)
    {
      return;
    }

    setPaginationData(prev => ({
      ...prev,
      page: paginationData.page - 1
    }));

    await handleFetchGreenhouses(
      null,
      paginationData.page - 1,
      paginationData.count);
  }

  const triggerMessage = (message) => {
    setErrorModalIsActive(true);
    setErrorMessage(message);

    setTimeout(() => {
      setErrorModalIsActive(false);
      setErrorMessage("");
    }, 3000)
  }

  return (
    <div className={styles.Home__Main}>
      <PopupModal isActive={modalIsActive} setIsActive={setModalIsActive}>
        <AddGreenhouse setNewGreenhouse={setAddGreenhouse}/>
      </PopupModal>
      <PopupModal isActive={errorModalIsActive} setIsActive={setErrorModalIsActive}>
        <p className={styles.Home__ErrorMessage}>{errorMessage}</p>
      </PopupModal>
      <Header/>
      <div className={styles.Home__Header_Welcome}>
        <h1 className={styles.Home__Header_WelcomeText}>Automation in the greenhouse: the future in agriculture</h1>
        <p className={styles.Home__Header_Text}>Automatic control increases productivity and reduces losses</p>
      </div>
      <div className={styles.Home__Greenhouses}>
        <h2 className={styles.Home__Greenhouses_HeaderText}>Greenhouses</h2>
        <div className={styles.Home__Greenhouses_Data}>
          <div className={styles.Home__Greenhouses_LeftBar}>
            <div className={styles.Home__Greenhouses_GreenhousesList}>
              <GreenhouseList greenhouses={greenhouses} selectGreenhouse={setCurrentGreenhouse}/>
            </div>
            <div className={styles.Home__Greenhouses_Pagination}>
              <Pagination handleNext={handleNextGreenhouses} 
                      handlePrev={handlePrevGreenhouses}/>
            </div>
          </div>
          <div className={styles.Home__Greenhouses_CenterBar}>
            {
              currentGreenhouse ? 
              <GreenhouseEvents greenhouse={currentGreenhouse} triggerMessage={triggerMessage}/> :
              <div className={styles.Home__Greenhouses_GreenhouseEmpty}>
                <p>Select any Greenhouse</p>
              </div>
            }
          </div>
        </div>
        <div className={styles.Home__Greenhouses_AddGreenhouse}>
          <div className={styles.Home__Greenhouses_BtnWrapper}>
            <PrimaryButton text={"Add Greenhouse"}
              action={() => setModalIsActive(true)}
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

export default Home;
