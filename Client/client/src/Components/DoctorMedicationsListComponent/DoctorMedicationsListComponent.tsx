import React, { useEffect, useState } from "react";
import "../DoctorMedicationsListComponent/doctorMedicationsListComponent.css";
import { url } from "../../Resources/Hosts";
import { Redirect } from "react-router-dom";
import AlertModal from "../AlertModal/AlertModal";
import IMedication from "../../Interfaces/IMedication";
import DoctorAddMedicationComponent from "./DoctorAddMedicationComponent/DoctorAddMedicationComponent";
import DoctorDisplayMedications from "./DoctorDisplayMedications/DoctorDisplayMedications";
import DoctorModifyMedicationComponent from "./DoctorModifyMedicationComponent/DoctorModifyMedicationComponent";
import { ICreateMedication } from "./DoctorAddMedicationComponent/DoctorAddMedicationComponent";

export default function DoctorMedicationsListComponent() {
    //let connectedDoctorMail = localStorage.getItem('email');
    const [medications, setMedications] = useState<IMedication[]>([]);
    const [errorMessage, setErrorMessage] = useState<null | string>(null);
    const [updateId, setUpdateId] = useState(0);
    const [isAuthorized, setIsAuthorized] = useState(true);

    function getMedications() {
        fetch(`${url}/medication`, {
            headers: {
                Authorization: `${localStorage.getItem("accessToken")}`,
            },
        }).then((response) => {
            if (response.status === 200) {
                response.json().then((result) => setMedications(result));
            } else if (response.status === 403) {
                setIsAuthorized(false);
            }
        });
    }

    function addMedication(medication: ICreateMedication) {
        fetch(`${url}/medication`, {
            method: "POST",
            body: JSON.stringify(medication),
            headers: {
                Authorization: `${localStorage.getItem("accessToken")}`,
                "Content-Type": "application/json",
            },
        }).then((response) => {
            if (response.status === 200) {
                setErrorMessage("");
                getMedications();
            } else {
                setErrorMessage("Ooops! We have some trouble with the server. Try again later.");
            }
        });
    }

    function modifyMedication(medication: IMedication) {
        fetch(`${url}/medication/${medication.id}`, {
            method: "PUT",
            body: JSON.stringify(medication),
            headers: {
                Authorization: `${localStorage.getItem("accessToken")}`,
                "Content-Type": "application/json",
            },
        }).then((response) => {
            if (response.status === 200) {
                setErrorMessage("");
                getMedications();
                setUpdateId(0);
            } else {
                setErrorMessage("Ooops! We have some trouble with the server. Try again later.");
            }
        });
    }

    function deleteMedication(id: number) {
        fetch(`${url}/medication/${id}`, {
            method: "DELETE",
            headers: {
                Authorization: `${localStorage.getItem("accessToken")}`,
            },
        }).then((response) => {
            if (response.status === 500) {
                setErrorMessage("Ooops! We have some trouble with the server. Try again later.");
            }
            if (response.status === 200) {
                setErrorMessage("");
                getMedications();
            }
        });
    }

    useEffect(() => {
        getMedications();
    }, []);

    return (
        <div className="card-container-medications">
            <DoctorAddMedicationComponent onAdd={addMedication} />
            {medications.map((medication: IMedication) =>
                medication.id !== updateId ? (
                    <DoctorDisplayMedications
                        key={medication.id}
                        medication={medication}
                        delete={deleteMedication}
                        modify={() => setUpdateId(medication.id)}
                    />
                ) : (
                        <DoctorModifyMedicationComponent
                            medication={medication}
                            cancel={() => setUpdateId(0)}
                            onModify={modifyMedication}
                        />
                    )
            )}
            {errorMessage === "" ? (
                <AlertModal message={"Action successful!"} closeModal={() => setErrorMessage(null)} title={"Success"} />
            ) : null}
            {errorMessage ? (
                <AlertModal message={errorMessage} closeModal={() => setErrorMessage(null)} title={"Error"} />
            ) : null}
            {!isAuthorized ? <Redirect to="/unauthorized" /> : null}
        </div>
    );
}