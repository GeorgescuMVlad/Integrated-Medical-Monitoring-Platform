import React from "react";
import "../DoctorDisplayCaregivers/doctorDisplayCaregivers.css";
import ICaregiver from "../../../Interfaces/ICaregiver";

interface IDisplayCaregiver {
    caregiver: ICaregiver;
    delete: (id: number) => void;
    modify: () => void;
    addPatients: () => void;
    deletePatients: () => void;
}

export default function DoctorDisplayCaregivers(props: IDisplayCaregiver) {

    return (
        <div className="caregiver-card">
            <div className="caregiver-name">{props.caregiver.name}</div>
            <div className="caregiver-details">
                <p>{`Birthday: ${props.caregiver.birthday}`}</p>
                <p>{`Address: ${props.caregiver.address}`}</p>
                <p>{`Gender: ${props.caregiver.gender}`}</p>
            </div>
            <div className="buttons-card">
                <button className="doctor-buttons delete" onClick={() => props.delete(props.caregiver.userId)}>
                    Delete
                </button>
                <button className="doctor-buttons modify" onClick={() => props.modify()}>
                    Update
                </button>
                <button className="doctor-buttons modify" onClick={() => props.addPatients()}>
                    Add Patients
                </button>
                <button className="doctor-buttons delete" onClick={() => props.deletePatients()}>
                    Delete Patients
                </button>
            </div>
        </div>
    );

}