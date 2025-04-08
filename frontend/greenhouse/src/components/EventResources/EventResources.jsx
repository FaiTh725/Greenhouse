import { useEffect, useState } from "react";
import styles from "./EventResources.module.css"
import PrimaryButton from "../buttons/PrimaryButton/PrimaryButton";
import ResourceCart from "../ResourceCart/ResourceCart";
import PopupModal from "../PopupModal/PopupModal";
import AddResource from "../AddResource/AddResource";

const EventResources = ({ 
          event, addResource, 
          updateResource, resources = []}) => {
  const [ modalAddResourceIsActive, setModaAddResourceIsActive] = useState(false);

  return (
    <div className={styles.EventResources__Main}>
      <PopupModal isActive={modalAddResourceIsActive} setIsActive={setModaAddResourceIsActive}>
        <AddResource addResource={addResource}/>
      </PopupModal>
      <div className={styles.EventResources__Wrapper}>
        <div className={styles.EventResources__Header}>
          {
            event.eventStatus == 0 && 
            <div className={styles.EventResources__AddBtnWrapper}>
              <PrimaryButton text={"Add Resource"}
                backgroundColor={"#2fee9e"}
                color={"#fff"}
                hoverBackgroundColor={"#000"}
                hoverColor={"#fff"}
                action={() => setModaAddResourceIsActive(true)}/>
            </div>
          }
        </div>
        <div className={styles.EventResources__Resources}>
          {
            resources.map(resource => 
              <ResourceCart key={resource.id} 
                  event={event} resource={resource}
                  updateResource={updateResource}/>
            )
          }
        </div>
      </div>
    </div>
  )
}

export default EventResources;