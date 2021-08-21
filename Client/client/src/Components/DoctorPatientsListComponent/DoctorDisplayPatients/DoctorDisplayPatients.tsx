import React from "react";
import "../DoctorDisplayPatients/doctorDisplayPatients.css";
import IPatient from "../../../Interfaces/IPatient";

interface IDisplayPatient {
    patient: IPatient;
    delete: (id: number) => void;
    modify: () => void;
}

export default function DoctorDisplayPatients(props: IDisplayPatient) {

    return (
        <div className="patient-card">
            <div className="patient-name">{props.patient.name}</div>
            <div className="patient-details">
                <p>{`Birthday: ${props.patient.birthday}`}</p>
                <p>{`Address: ${props.patient.address}`}</p>
                <p>{`Gender: ${props.patient.gender}`}</p>
                <p>{`Medical record: ${props.patient.medicalRecord}`}</p>
            </div>
            <div className="buttons-card">
                <button className="doctor-buttons delete" onClick={() => props.delete(props.patient.userId)}>
                    Delete
                </button>
                <button className="doctor-buttons modify" onClick={() => props.modify()}>
                    Update
                </button>
            </div>
        </div>
    );

}