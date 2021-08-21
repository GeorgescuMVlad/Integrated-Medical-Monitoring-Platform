import React, { useState, useEffect } from "react";
import { Redirect } from "react-router-dom";
import IMedicalPlansDetails from "../../../Interfaces/IMedicalPlansDetails";
import IPatient from "../../../Interfaces/IPatient";
import { url } from "../../../Resources/Hosts";
import AlertModal from "../../AlertModal/AlertModal";
import PatientDisplayMedicationPlans from "../PatientDisplayMedicationPlans/PatientDisplayMedicationPlans";
import "../PatientGetMedicationPlans/patientGetMedicationPlans.css";

interface IDisplayPatients {
    patients: IPatient[];
    connectedPatientMail: string | null;
    userId: number;
    getConnectedPatient: (connectedPatient: string | null) => void;
}

export default function PatientGetMedicationPlans(props: IDisplayPatients) {
    const [medicationPlans, setMedicationPlans] = useState<IMedicalPlansDetails[]>([]);
    const [isAuthorized, setIsAuthorized] = useState(true);
    const [errorMessage, setErrorMessage] = useState<null | string>(null);

    function getPatientMedicationPlans(userId: number) {
        fetch(`${url}/medicationPlan/${userId}`, {
            headers: {
                Authorization: `${localStorage.getItem("accessToken")}`,
            },
        }).then((response) => {
            if (response.status === 200) {
                response.json().then((result) => setMedicationPlans(result));
            } else if (response.status === 403) {
                setIsAuthorized(false);
            }
        });
    }

    useEffect(() => {
        getPatientMedicationPlans(props.userId);
    }, [props.userId]);

    return (
        medicationPlans.length > 0 ? (
            <div className="card-container-medication-plans">
                {medicationPlans.map((medicationPlan: IMedicalPlansDetails) => (
                    <PatientDisplayMedicationPlans
                        key={medicationPlan.dosage}
                        medicationPlan={medicationPlan}
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
                <div className="patient-information">Welcome! You have no Medication Plans assigned yet!</div>
            )
    );
}