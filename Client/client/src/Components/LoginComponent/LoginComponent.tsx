import React, { useState } from 'react';
import "../LoginComponent/loginComponent.css";
import ICredentials from "../../Interfaces/ICredentials";
import { url } from "../../Resources/Hosts";
import { Redirect } from "react-router-dom";
import logo from "../../Resources/images/logoapp.png";

export interface ICreateCredentials {
    email: string;
    password: string;
}

export default function LoginComponent() {
    const [credentials, setCredentials] = useState<ICreateCredentials>({
        email: "",
        password: "",
    });
    const [logged, setLogged] = useState(false);
    const [error, setError] = useState(false);
    const [errorMessage, setErrorMessage] = useState("");
    const [userType, setUserType] = useState("");

    function authenticate(credentials: ICredentials) {
        fetch(`${url}/user`, {
            method: "POST",
            body: JSON.stringify(credentials),
            headers: {
                "Content-Type": "application/json",
            },
        }).then((response) => {
            if (response.status === 200) {
                response.text().then(function (result) {
                    let resultSet = JSON.parse(result);
                    localStorage.setItem("accessToken", resultSet[0]);
                    localStorage.setItem("email", credentials.email);
                    setUserType(resultSet[1]);
                    setLogged(true);
                });
            } else {
                if (response.status === 401) {
                    setError(true);
                    setErrorMessage("Oops! Looks like this email and password combo is invalid. Feel free to try again.");
                }
                if (response.status === 500) {
                    setError(true);
                    setErrorMessage("Oops! Looks like the error is on our side. Don't worry, we'll fix it soon!");
                }
            }
        });
    }

    function updateProperty(property: string, newValue: string) {
        let newCredentials: any = Object.assign({}, credentials);
        newCredentials[property] = newValue;
        setCredentials(newCredentials);
    }

    return (
        <div className="login-container">
            <img src={logo} className="logo" alt="" />
            <input
                className="login-inputs"
                type={"text"}
                placeholder="E-mail"
                value={credentials.email}
                onChange={(e) => updateProperty("email", e.target.value)}
                onKeyPress={(event) => { if (event.key === "Enter") { authenticate(credentials); } }}
            />
            <input
                className="login-inputs"
                type={"password"}
                placeholder="Password"
                value={credentials.password}
                onChange={(e) => updateProperty("password", e.target.value)}
                onKeyPress={(event) => { if (event.key === "Enter") { authenticate(credentials); } }}
            />
            <button
                className="login-button"
                onClick={() => { authenticate(credentials); }}
            >
                Log in
            </button>
            {error ? (
                <div className="login-alert">
                    <span className="login-alert-close-button" onClick={() => setError(false)}>
                        &times;
                    </span>
                    {errorMessage}
                </div>
            ) : null}
            {logged && userType === "doctor" ? <Redirect to="/doctor" /> : null}
            {logged && userType === "caregiver" ? <Redirect to="/caregiver" /> : null}
            {logged && userType === "patient" ? <Redirect to="/patient" /> : null}
        </div>
    );
}