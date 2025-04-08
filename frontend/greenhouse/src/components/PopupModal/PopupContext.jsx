import { createContext, useContext, useState } from "react";
import PopupModal from "./PopupModal";

const PopupContext = createContext();

export const PopupProvider = ({children}) => {
  const [ isActive, setIsActive ] = useState(false);
  const [ content, setContent] = useState(null);

  const show = (content) => {
    setContent(content);
    setIsActive(true);
  }

  const close = () => {
    setContent(null);
    setIsActive(false);
  }

  return (
    <PopupContext.Provider value={{show, close}}>
      {children}
      <PopupModal isActive={isActive} setIsActive={setIsActive}>
        {content}
      </PopupModal>
    </PopupContext.Provider>
  )
}

export const usePopup = () => useContext(PopupContext);