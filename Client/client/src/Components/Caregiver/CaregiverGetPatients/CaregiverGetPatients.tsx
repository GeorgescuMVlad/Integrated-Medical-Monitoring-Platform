import React, { useState, useEffect } from "react";
import ICaregiver from "../../../Interfaces/ICaregiver";
import { Redirect } from "react-router-dom";
import IPatient from "../../../Interfaces/IPatient";
import { url } from "../../../Resources/Hosts";
import AlertModal from "../../AlertModal/AlertModal";
import CaregiverDisplayPatients from "../CaregiverDisplayPatients/CaregiverDisplayPatients";
import "../CaregiverGetPatients/caregiverGetPatients.css";

interface IDisplayPatients {
    caregivers: ICaregiver[];
    connectedCarregiverMail: string | null;
    userId: number;
    getConnectedCaregiver: (connectedCaregiver: string | null) => void;
}

export default function CaregiverGetPatients(props: IDisplayPatients) {
    const [patients, setPatients] = useState<IPatient[]>([]);
    const [isAuthorized, setIsAuthorized] = useState(true);
    const [errorMessage, setErrorMessage] = useState<null | string>(null);

    function getPatientsCaredByCaregiver(userId: number) {
        fetch(`${url}/caregiver/${userId}`, {
            headers: {
                Authorization: `${localStorage.getItem("accessToken")}`,
            },
        }).then((response) => {
            if (response.status === 200) {
                response.json().then((result) => setPatients(result));
            } else if (response.status === 403) {
                setIsAuthorized(false);
            }
        });
    }

    useEffect(() => {
        getPatientsCaredByCaregiver(props.userId);
    }, [props.userId]);

    return (
        patients.length > 0 ? (
            <div className="card-container-patients">
                {patients.map((patient: IPatient) => (
                    <CaregiverDisplayPatients
                        key={patient.id}
                        patient={patient}
                    />
                ))}
                {errorMessage === "" ? (
                    <AlertModal message={"Action successful!"} closeModal={() => setErrorMessage(null)} title={"Success"} />
                ) : null}
                {errorMessage ? (
                    <AlertModal message={errorMessage} closeModal={() => setErrorMessage(null)} title={"Error"} />
                ) : null}
                {!isAuthorized ? <Redirect to="/unauthorized" /> : null}
            </div>
        ) : (
                <div className="caregiver-information">Welcome! You have no Patients to care yet!</div>
            )
    );
}