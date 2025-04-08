import useTranslateResourceType from "../../services/useTranslateResourceType";
import Counter from "../inputs/Counter/Counter";
import styles from "./ResourceCart.module.css";

const ResourceCart = ({event, resource, updateResource}) => {
  
  const handleUpdateActualAmount = (resourceActualCount) => {
    updateResource({
      ...resource,
      actualAmount: resourceActualCount
    });
  }

  return (
    <div className={styles.ResourceCart__Main}>
      <p className={styles.ResourceCart__Name}>{resource.name}</p>
      <p className={styles.ResourceCart__Unit}>{resource.unit}</p>
      <p className={styles.ResourceCart__ResourceType}>{useTranslateResourceType(resource.resourceType)}</p>
      <p className={styles.ResourceCart__PlannedAmount}>{resource.plannedAmount}</p>
      <div className={styles.ResourceCart__ActualAmount}>
        {
          event.eventStatus == 1 ? 
          <Counter value={resource.actualAmount ? resource.actualAmount : 0} 
            minValue={0}
            setValue={handleUpdateActualAmount}/> :
          resource.actualAmount
        }
      </div>
    </div>
  )
}

export default ResourceCart;