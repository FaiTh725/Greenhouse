const useTranslateEventStatus = (eventStatus) => {
  switch (eventStatus) {
    case 0:
      return "Planned";
    case 1:
      return "In Progress";
    case 2:
      return "Complete";
    default:
      return "Invalid Status"
  }
}

export default useTranslateEventStatus; 