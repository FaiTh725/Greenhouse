import { useState } from "react";
import PrimaryButton from "../buttons/PrimaryButton/PrimaryButton";
import Combobox from "../inputs/Combobox/Combobox";
import PrimaryInput from "../inputs/PrimaryInput/PrimaryInput";
import styles from "./AddResource.module.css"

const AddResource = ({addResource}) => {
  const [ form, setForm ] = useState({
    name: "",
    unit: "",
    plannedAmount: "",
    resourceType: null,
  });
  const [ erroForm, setErrorForm ] = useState({
    nameError: "",
    unitError: "",
    plannedAmountError: "",
    resourceTypeError: "",
  });

  const resourceTypes = [
    {
      type: 0,
      name: "Seed"
    },
    {
      type: 1,
      name: "Fertilizer"
    },
    {
      type: 2,
      name: "Water"
    },
  ];

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
      unitError: "",
      plannedAmountError: "",
      resourceTypeError: "",
    });
  }

  const handleSelectResourceType = (value) => {
    setForm(prev => ({
      ...prev,
      resourceType: value
    }));
  }

  const handleAddResource = async (e) => {
    e.preventDefault();

    let formIsValid = true;
  
    if(form.name.length == 0)
    {
      setErrorForm(prev => ({
        ...prev,
        nameError: "Name is required"
      }));
      formIsValid = false;
    }
    if(form.unit.length == 0)
    {
      setErrorForm(prev => ({
        ...prev,
        unitError: "Unit is required"
      }));
      formIsValid = false;
    }
    if(form.resourceType == null)
    {
      setErrorForm(prev => ({
        ...prev,
        resourceTypeError: "Select any type resource"
      }));
      formIsValid = false;
    }
    const plannedAmount = Number(form.plannedAmount);
    if( !form.plannedAmount || isNaN(plannedAmount) ||
        plannedAmount < 0)
    {
      setErrorForm(prev => ({
        ...prev,
        plannedAmountError: "Enter correct amount"
      }));
      formIsValid = false;
    }

    if(!formIsValid)
    {
      return;
    }

    const resourceType = resourceTypes.find(x => x.name == form.resourceType).type;

    await addResource({
        plannedAmount: plannedAmount,
        name: form.name,
        unit: form.unit,
        resourceType: resourceType,
    });

    setForm({
      name: "",
      unit: "",
      plannedAmount: "",
      resourceType: null,
    });
  }

  return (
    <div className={styles.AddResource__Main}>
      <div className={styles.AddResource__Wrapper}>
        <div className={styles.AddResource__Form}>
          <div className={styles.AddResource__FromInput}>
            <PrimaryInput placeholder="Name"
              value={form.name}
              inputName="name"
              setValue={handleChangeForm}
              errorMessage={erroForm.nameError}
              clearError={handleClearErrorForm}/>
          </div>
          <div className={styles.AddResource__FromInput}>
            <PrimaryInput placeholder="Unit"
              value={form.unit}
              inputName="unit"
              setValue={handleChangeForm}
              errorMessage={erroForm.unitError}
              clearError={handleClearErrorForm}/>
          </div>
          <div className={styles.AddResource__FromInput}>
            <PrimaryInput placeholder="Planned Amount"
              value={form.plannedAmount}
              inputName="plannedAmount"
              setValue={handleChangeForm}
              errorMessage={erroForm.plannedAmountError}
              clearError={handleClearErrorForm}/>
          </div>
          <div className={styles.AddResource__FromInput}>
            <Combobox items={resourceTypes.map(type => type.name)}
              selectedItem={form.resourceType}
              setSelectedItem={handleSelectResourceType}
              errorMessage={erroForm.nameError}
              clearError={handleClearErrorForm}/>
          </div>
        </div>
        <div className={styles.AddResource__BtnAdd}>
          <PrimaryButton text={"Add Resource"}
            action={handleAddResource}
            backgroundColor={"#2fee9e"}
            color={"#fff"}
            hoverBackgroundColor={"#000"}
            hoverColor={"#fff"}/>
        </div>
      </div>
    </div>
  )
}

export default AddResource;