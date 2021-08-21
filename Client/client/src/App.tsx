import React, { useEffect, useState } from 'react';
import './App.css';
import { url } from "./Resources/Hosts";
import { BrowserRouter as Router, Switch, Route, Redirect } from "react-router-dom";
import LoginComponent from './Components/LoginComponent/LoginComponent';
import HeaderComponent from './Components/HeaderComponent/HeaderComponent';
import "bootstrap/dist/css/bootstrap.min.css";
import Doctor from './Components/Doctor/Doctor';
import Caregiver from './Components/Caregiver/Caregiver';
import Patient from './Components/Patient/Patient';
import UnauthorizedPageComponent from './Components/UnauthorizedPageComponent/UnauthorizedPageComponent';
import DoctorPatientsListComponent from './Components/DoctorPatientsListComponent/DoctorPatientsListComponent';
import DoctorCaregiversListComponent from './Components/DoctorCaregiversListComponent/DoctorCaregiversListComponent';
import DoctorMedicationsListComponent from './Components/DoctorMedicationsListComponent/DoctorMedicationsListComponent';

function App() {
  const [userType, setUserType] = useState("");

  function getUserType() {
    let token = localStorage.getItem("accessToken") ?? "";
    fetch(`${url}/user/type`, {
      method: "POST",
      body: JSON.stringify(token),
      headers: {
        "Content-Type": "application/json",
      },
    }).then((response) => {
      if (response.status === 200) {
        response.text().then((result) => {
          setUserType(result);
        });
      }
    });
  }

  useEffect(() => {
    getUserType();
  }, []);

  return (
    <div >
      <Router>
        <HeaderComponent />
        <Switch>
          <Route exact path="/">
            {userType === "none" ? (
              <LoginComponent />
            ) : userType === "doctor" ? (
              <Redirect exact to="/doctor" />
            ) : userType === "caregiver" ? (
              <Redirect exact to="/caregiver" />
            ) : userType === "patient" ? (
              <Redirect exact to="/patient" />
            ) : (<LoginComponent />)}
          </Route>
          <Route exact path="/doctor">
            <Doctor />
          </Route>
          <Route exact path="/caregiver">
            <Caregiver />
          </Route>
          <Route exact path="/patient">
            <Patient />
          </Route>
          <Route exact path="/doctor/patients">
            <DoctorPatientsListComponent />
          </Route>
          <Route exact path="/doctor/caregivers">
            <DoctorCaregiversListComponent />
          </Route>
          <Route exact path="/doctor/medications">
            <DoctorMedicationsListComponent />
          </Route>
          <Route exact path="/unauthorized">
            <UnauthorizedPageComponent />
          </Route>
        </Switch>
      </Router>
    </div>
  );
}

export default App;
