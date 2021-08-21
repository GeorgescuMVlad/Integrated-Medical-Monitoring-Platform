import React from "react";
import "../UnauthorizedPageComponent/unauthorizedPageComponent.css";
import ErrorSvg from "../../Resources/images/Space.svg";

export default function UnauthorizedPageComponent() {
    return (
        <div className="unauthorized-page-container">
            <h1 className="error-title">403</h1>
            <h2>We're sorry, but you do not have access to this page or resource.
            </h2>
            <img
                className="error-illustration"
                src={ErrorSvg}
                alt="403 error illustration"
            />
        </div>
    );
}