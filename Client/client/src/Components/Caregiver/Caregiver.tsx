import React, { useEffect, useState, useCallback } from "react";
import { url } from "../../Resources/Hosts";
import { Redirect } from "react-router-dom";
import ICaregiver from "../../Interfaces/ICaregiver";
import CaregiverGetPatients from "./CaregiverGetPatients/CaregiverGetPatients";

export default function Caregiver() {
    let connectedCarregiverMail = localStorage.getItem('email');
    const [caregivers, setCaregivers] = useState<ICaregiver[]>([]);
    const [isAuthorized, setIsAuthorized] = useState(true);
    const [userId, setUserId] = useState<number>(0);

    function getCaregivers() {
        fetch(`${url}/caregiver`, {
            headers: {
                Authorization: `${localStorage.getItem("accessToken")}`,
            },
        }).then((response) => {
            if (response.status === 200) {
                response.json().then((result) => setCaregivers(result));
            } else if (response.status === 403) {
                setIsAuthorized(false);
            }
        });
    }

    function getConnectedCaregiver(caregiverMail: string | null) {
        fetch(`${url}/user/${caregiverMail}`, {
            headers: {
                Authorization: `${localStorage.getItem("accessToken")}`,
            },
        }).then((response) => {
            if (response.status === 200) {
                response.json().then((result) => { setUserId(result.id) });
            } else if (response.status === 403) {
                setIsAuthorized(false);
            }
        });
    }

    useEffect(() => {
        getCaregivers();
    }, []);

    const getConnectedCaregiverCallback = useCallback(() => {
        getConnectedCaregiver(connectedCarregiverMail)
    }, [connectedCarregiverMail]);

    useEffect(() => {
        getConnectedCaregiverCallback();
    }, [getConnectedCaregiverCallback]);


    return (
        <div>
            {caregivers.map((caregiver: ICaregiver) =>
                caregiver.userId === userId ?
                    <CaregiverGetPatients
                        key={caregiver.id}
                        caregivers={caregivers}
                        connectedCarregiverMail={connectedCarregiverMail}
                        userId={caregiver.id}
                        getConnectedCaregiver={getConnectedCaregiver}
                    /> : null
            )}
            {!isAuthorized ? <Redirect to="/unauthorized" /> : null}
        </div>
    );
}