const useDateTimeFormat = (datetime) => {
  const date = new Date(datetime);

  const day = date.toLocaleDateString("fr-CA").split("T")[0];
  const hours = date.getHours();
  const minutes = date.getMinutes();

  return `${day} ${hours}:${minutes}`;
}


export default useDateTimeFormat;