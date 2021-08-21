import React from "react";
import "../CaregiverDisplayPatients/caregiverDisplayPatients.css";
import IPatient from "../../../Interfaces/IPatient";

interface IDisplayCaregiver {
    patient: IPatient;
}

export default function CaregiverDisplayPatients(props: IDisplayCaregiver) {

    return (
        <div className="patient-card">
            <div className="patient-name">{props.patient.name}</div>
            <div className="patient-details">
                <p>{`Birthday: ${props.patient.birthday}`}</p>
                <p>{`Address: ${props.patient.address}`}</p>
                <p>{`Gender: ${props.patient.gender}`}</p>
                <p>{`Medical Record: ${props.patient.medicalRecord}`}</p>
            </div>
        </div>
    );
}