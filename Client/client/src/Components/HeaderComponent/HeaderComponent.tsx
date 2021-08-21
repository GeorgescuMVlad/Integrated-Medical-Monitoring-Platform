import React, { useState } from 'react';
import "../../Components/HeaderComponent/headerComponent.css";
import { BiLogOut, BiHome } from "react-icons/bi";
import { Link, Redirect, useLocation } from "react-router-dom";

export default function HeaderComponent() {
    const [redirect, setRedirect] = useState(false);
    const location = useLocation();
    const pagesWithoutLoginButton = ["/"];
    const pagesWithHomeButton = ["/doctor/patients", "/doctor/caregivers", "/doctor/medications"];

    function routeChange() {
        //localStorage.removeItem("accessToken");
        localStorage.clear();
        setRedirect(true);
    }

    return (
        <div>
            {redirect ? <Redirect exact to="/" /> : null}
            <header className="app-header">
                <div className="left-container">
                    {pagesWithHomeButton.includes(location.pathname) ? (
                        <Link to="/doctor" className="home-link">
                            <BiHome className="icon home" title="Dashboard"></BiHome>
                        </Link>
                    ) : ("")}
                </div>
                <div className="right-container">
                    {!pagesWithoutLoginButton.includes(location.pathname) ? (
                        <BiLogOut className="icon logout" onClick={routeChange} title="Log out"></BiLogOut>
                    ) : ("")}
                </div>
            </header>
        </div>
    );
}