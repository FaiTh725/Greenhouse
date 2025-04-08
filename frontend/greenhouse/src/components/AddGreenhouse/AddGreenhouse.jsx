import { useState } from "react";
import PrimaryButton from "../buttons/PrimaryButton/PrimaryButton";
import PrimaryInput from "../inputs/PrimaryInput/PrimaryInput";
import styles from "./AddGreenhouse.module.css";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../Auth/AuthContext";

const AddGreenhouse = ({setNewGreenhouse}) => {
  const [ form, setForm ] = useState({
    name: "",
    area: "",
    location: "",
    cropName: ""
  });
  const [ formError, setFormError ] = useState({
    nameError: "",
    areaError: "",
    locationError: "",
    cropNameError: ""
  });

  const navigate = useNavigate();
  const auth = useAuth();


  const handleChangeForm = (e) => {
    const key = e.target.name;
    const newValue = e.target.value;

    setForm(prev => ({
      ...prev,
      [key] : newValue
    }));
  }

  const handleClearErorForm = () => 
    setFormError({
      nameError: "",
      areaError: "",
      locationError: "",
      cropNameError: ""
    })
   
  const handleAddGreenhouse = async (e) => {
    e.preventDefault();

    var isValidForm = true;

    if(form.name.length == 0)
    {
      setFormError(prev => ({
        ...prev,
        nameError: "Name is Required"
      }));

      isValidForm = false;
    }
    if(form.location.length == 0)
    {
      setFormError(prev => ({
        ...prev,
        nameError: "Location is required"
      }));

      isValidForm = false;
    }
    if(form.area.length == 0)
    {
      setFormError(prev => ({
        ...prev,
        nameError: "Area is required"
      }));

      isValidForm = false;
    }
    if(form.cropName.length == 0)
    {
      setFormError(prev => ({
        ...prev,
        nameError: "Crop Name is required"
      }));

      isValidForm = false;
    }
    if(Number.parseFloat(form.area) < 0)
    {
      setFormError(prev => ({
        ...prev,
        nameError: "Area less than zero"
      }));

      isValidForm = false;
    }

    if(!isValidForm)
    {
      return;
    }

    try
    {
      console.log(form);
      const response = await axios.post("https://localhost:5202/api/Greenhouse/CreateGreenhouse", {
        greenhouseName: form.name,
        area: form.area,
        location: form.location,
        cropName: form.cropName
      }, {
        headers: {
          "Content-Type" : "application/json"
        },
        withCredentials: true
      });
      
      setNewGreenhouse({
        area: response.data.area,
        id: response.data.id,
        location: response.data.location,
        cropName: response.data.cropName,
        nameGreenhouse: response.data.nameGreenHouse
      });

      setForm({
        name: "",
        area: "",
        location: "",
        cropName: ""
      });
    }
    catch (error)
    {
      console.log(error);
      if(error.response.status == 400)
      {
        setFormError(prev => ({
          ...prev,
          nameError: error.response.data.detail
        }));
      }
      else if(error.response.status == 401 ||
        error.response.status == 409)
      {
        auth.logout();
        navigate("/login");
      }
    }
  }
  
  return (
    <div className={styles.AddGreenhouse__Main}>
      <div className={styles.AddGreenhouse__Wrapper}>
        <div className={styles.AddGreenhouse__Form}>
          <PrimaryInput 
            value={form.name} placeholder="Name" 
            setValue={handleChangeForm} errorMessage={formError.nameError}
            clearError={handleClearErorForm} inputName="name"/>
          <PrimaryInput
            value={form.area} placeholder="Area" 
            setValue={handleChangeForm} errorMessage={formError.areaError}
            clearError={handleClearErorForm} inputName="area"/>
          <PrimaryInput
            value={form.location} placeholder="Location" 
            setValue={handleChangeForm} errorMessage={formError.locationError}
            clearError={handleClearErorForm} inputName="location"/>
          <PrimaryInput
            value={form.cropName} placeholder="Crop Name" 
            setValue={handleChangeForm} errorMessage={formError.cropNameError}
            clearError={handleClearErorForm} inputName="cropName"/>
        </div>
        <div className={styles.AddGreenhouse__SendFormBtn}>
          <PrimaryButton text={"Add"}
            action={handleAddGreenhouse}
            backgroundColor={"#2fee9e"}
            color={"#fff"}
            hoverBackgroundColor={"#000"}
            hoverColor={"#fff"}/>
        </div>
      </div>
    </div>
  )
}

export default AddGreenhouse;