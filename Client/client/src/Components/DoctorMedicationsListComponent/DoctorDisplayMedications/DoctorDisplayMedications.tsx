import React from "react";
import "../DoctorDisplayMedications/doctorDisplayMedications.css";
import IMedication from "../../../Interfaces/IMedication";

interface IDisplayMedication {
    medication: IMedication;
    delete: (id: number) => void;
    modify: () => void;
}

export default function DoctorDisplayMedications(props: IDisplayMedication) {

    return (
        <div className="medication-card">
            <div className="medication-name">{props.medication.name}</div>
            <div className="medication-details">
                <p>{`Side effects: ${props.medication.sideEffects}`}</p>
                <p>{`Dosage: ${props.medication.dosage}`}</p>
            </div>
            <div className="buttons-card">
                <button className="doctor-buttons delete" onClick={() => props.delete(props.medication.id)}>
                    Delete
                </button>
                <button className="doctor-buttons modify" onClick={() => props.modify()}>
                    Update
                </button>
            </div>
        </div>
    );

}