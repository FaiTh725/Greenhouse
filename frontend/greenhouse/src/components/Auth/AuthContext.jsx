import { createContext, useContext, useEffect, useState } from "react";

const AuthContext = createContext();

export const AuthProvider = ({children}) => {
  const [user, setUser] = useState(undefined);

  const login = (userData) => {
    localStorage.setItem("auth", JSON.stringify(userData));
    setUser(userData);
  }

  const logout = () => {
    localStorage.removeItem("auth");
    setUser(null);
  }

  useEffect(() => {
    try
    {
      const authData = JSON.parse(localStorage.getItem("auth"));
      if(authData)
      {
        setUser(authData);
      }
      else
      {
        setUser(null);
      }
    }
    catch
    {
      setUser(null);
    }
  }, []);


  return (
    <AuthContext.Provider value={{user, login, logout}}>
      {children}
    </AuthContext.Provider>
  )
  // if(user == undefined)
  // {
  //   return(
  //     <div style={{"width": "100vw", "height": "100vh", "display": "flex", "justifyContent": "center", "alignItems": "center"}}>
  //       Loading
  //     </div>
  //   )
  // }
  // else
  // {
  //   return (
  //     <AuthContext.Provider value={{user, login, logout}}>
  //       {children}
  //     </AuthContext.Provider>
  //   )
  // }
}

export const useAuth = () => useContext(AuthContext);