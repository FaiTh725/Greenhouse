import { useAuth } from "./AuthContext"
import { Navigate, Outlet } from "react-router-dom";

const ProtectedRoutes = ({roles}) => {
  const auth = useAuth();

  if(auth.user === undefined)
  {
    return (
      <div style={{width: "100vw", height: "100vh", display: "flex", justifyContent: "center", alignItems: "center"}}>
        Loading
      </div>
    )
  }

  if(auth.user == null || !roles.includes(auth.user.role))
  {
    return <Navigate to="/login"/>
  }

  return <Outlet/>
}

export default ProtectedRoutes;