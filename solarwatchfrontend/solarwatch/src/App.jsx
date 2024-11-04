import { useState } from 'react'
import Register from "./pages/Register.jsx";
import {BrowserRouter, Route, Routes} from "react-router-dom"
import {CookiesProvider, useCookies} from 'react-cookie';
import Login from "./pages/Login.jsx";
import {Toaster} from "react-hot-toast";
import SolarForecast from "./pages/SolarForecast.jsx";

function App() {
    const [cookies, setCookies] = useCookies(['user'])

    function setUserLoginCookies(user) {
        setCookies('user', user, {path: '/'})
    }

  return (
      <CookiesProvider>
          <BrowserRouter>
              <Routes>
                  <Route path="/Register" element={<Register/>} />
                  <Route path="/Login" element={<Login cookies={cookies} setUserLoginCookies={setUserLoginCookies} />} />
                  <Route path="/" element={<SolarForecast cookies={cookies} setUserLoginCookies={setUserLoginCookies} />} />
              </Routes>
              <Toaster/>
          </BrowserRouter>
      </CookiesProvider>
  )
}

export default App
