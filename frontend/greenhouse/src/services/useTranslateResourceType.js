const useTranslateResourceType = (resourceType) => {
  switch (resourceType) {
    case 0:
      return "Seed";
    case 1:
      return "Fertilizer";
    case 2:
      return "Water";
    default: 
      return "Invalid Type"
  }
}

export default useTranslateResourceType;