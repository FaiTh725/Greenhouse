import { BrowserRouter, Route, Routes } from 'react-router-dom'
import './App.css'
import { AuthProvider } from './components/Auth/AuthContext'
import Login from './pages/Login/Login'
import Signin from './pages/Signin/Signin'
import ProtectedRoutes from './components/Auth/ProtectedRoutes'
import Home from './pages/Home/Home'
import ConfirmEmail from './pages/ConfirmEmail/ConfirmEmail'
import Profile from './pages/Profile/Profile'
import Report from './pages/Report/Report'

function App() {

  return (
    <BrowserRouter>
      <AuthProvider>
        <Routes>
          <Route path='/login' element={<Login/>}/>
          <Route path='/signin' element={<Signin/>}/>
          <Route path='/confirm' element={<ConfirmEmail/>}/>
          <Route element={<ProtectedRoutes roles={["Manager", "Admin"]}/>}>
            <Route path='/' element={<Home/>}/>
            <Route path='/profile' element={<Profile/>}/>
          </Route>
          <Route element={<ProtectedRoutes roles={["Admin"]}/>}>
            <Route path='/report' element={<Report/>}/>
          </Route>
        </Routes>
      </AuthProvider>
    </BrowserRouter>
  )
}

export default App
