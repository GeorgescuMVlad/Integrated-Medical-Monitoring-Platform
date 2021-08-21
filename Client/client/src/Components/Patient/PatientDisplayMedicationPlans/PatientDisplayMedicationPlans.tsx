import React from "react";
import IMedicationPlansDetails from "../../../Interfaces/IMedicalPlansDetails";
import "../PatientDisplayMedicationPlans/patientDisplayMedicationPlans.css";

interface IDisplayMedicationPlans {
    medicationPlan: IMedicationPlansDetails;
}

export default function PatientDisplayMedicationPlans(props: IDisplayMedicationPlans) {

    return (
        <div className="medication-plan-card">
            <div className="plan-medication-name">{props.medicationPlan.medicationName}</div>
            <div className="medication-plan-details">
                <p>{`Dosage: ${props.medicationPlan.dosage}`}</p>
                <p>{`Intake Interval: ${props.medicationPlan.intakeInterval}`}</p>
                <p>{`Period of Treatment: ${props.medicationPlan.periodOfTreatment}`}</p>
            </div>
        </div>
    );
}