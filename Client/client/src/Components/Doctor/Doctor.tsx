import React, { useEffect, useState } from "react";
import "../Doctor/doctor.css";
import { url } from "../../Resources/Hosts";
import { Link, Redirect } from "react-router-dom";

export default function Doctor() {
    const [noOfPatients, setNoOfPatients] = useState(0);
    const [noOfCaregivers, setNoOfCaregivers] = useState(0);
    const [noOfMedications, setNoOfMedications] = useState(0);
    const [isAuthorized, setIsAuthorized] = useState(true);
    //let connectedDoctorMail = localStorage.getItem('email');

    function getCount(name: string, setResult: (value: number) => void) {
        fetch(`${url}/${name}/count`, {
            method: "GET",
            headers: {
                Authorization: `${localStorage.getItem("accessToken")}`,
            },
        }).then((response) => {
            if (response.status === 200) {
                response.json().then((result) => {
                    setResult(result);
                });
            } else if (response.status === 403) {
                setIsAuthorized(false);
            }
        });
    }

    useEffect(() => {
        getCount("patient", setNoOfPatients);
        getCount("caregiver", setNoOfCaregivers);
        getCount("medication", setNoOfMedications);
    }, []);

    return (
        <div>
            <p className="dashboard-title">Doctor's Dashboard</p>
            <div className="dashboard-container">
                <Link to="/doctor/patients" className="link">
                    <div className="circle" title="See all patients">
                        <p className="count">{noOfPatients}</p>
                        <p className="text">PATIENTS</p>
                    </div>
                </Link>
                <Link to="/doctor/caregivers" className="link">
                    <div className="circle" title="See all caregivers">
                        <p className="count">{noOfCaregivers}</p>
                        <p className="text">CAREGIVERS</p>
                    </div>
                </Link>
                <Link to="/doctor/medications" className="link">
                    <div className="circle" title="See all medications">
                        <p className="count">{noOfMedications}</p>
                        <p className="text">MEDICATIONS</p>
                    </div>
                </Link>
                {!isAuthorized ? <Redirect to="/unauthorized" /> : null}
            </div>
        </div>
    );
}