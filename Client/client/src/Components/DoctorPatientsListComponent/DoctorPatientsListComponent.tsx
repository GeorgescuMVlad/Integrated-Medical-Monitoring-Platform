import React, { useEffect, useState } from "react";
import "../DoctorPatientsListComponent/doctorPatientsListComponent.css";
import { url } from "../../Resources/Hosts";
import { Redirect } from "react-router-dom";
import AlertModal from "../AlertModal/AlertModal";
import IPatient from "../../Interfaces/IPatient";
import DoctorDisplayPatients from "./DoctorDisplayPatients/DoctorDisplayPatients";
import DoctorAddPatientComponent, { ICreatePatientAndAccount } from "../DoctorPatientsListComponent/DoctorAddPatientComponent/DoctorAddPatientComponent";
import DoctorModifyPatientComponent from "./DoctorModifyPatientComponent/DoctorModifyPatientComponent";

export default function DoctorPatientsListComponent() {
    //let connectedDoctorMail = localStorage.getItem('email');
    const [patients, setPatients] = useState<IPatient[]>([]);
    const [errorMessage, setErrorMessage] = useState<null | string>(null);
    const [updateId, setUpdateId] = useState(0);
    const [isAuthorized, setIsAuthorized] = useState(true);

    function getPatients() {
        fetch(`${url}/patient`, {
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

    function addPatientWithAccount(patientAndAccount: ICreatePatientAndAccount) {
        fetch(`${url}/patient`, {
            method: "POST",
            body: JSON.stringify(patientAndAccount),
            headers: {
                Authorization: `${localStorage.getItem("accessToken")}`,
                "Content-Type": "application/json",
            },
        }).then((response) => {
            if (response.status === 200) {
                setErrorMessage("");
                getPatients();
            } else {
                setErrorMessage("Ooops! We have some trouble with the server. Try again later.");
            }
        });
    }

    function modifyPatient(patient: IPatient) {
        fetch(`${url}/patient/${patient.id}`, {
            method: "PUT",
            body: JSON.stringify(patient),
            headers: {
                Authorization: `${localStorage.getItem("accessToken")}`,
                "Content-Type": "application/json",
            },
        }).then((response) => {
            if (response.status === 200) {
                setErrorMessage("");
                getPatients();
                setUpdateId(0);
            } else {
                setErrorMessage("Ooops! We have some trouble with the server. Try again later.");
            }
        });
    }

    function deletePatient(id: number) {
        fetch(`${url}/patient/${id}`, {
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
                getPatients();
            }
        });
    }

    useEffect(() => {
        getPatients();
    }, []);

    return (
        <div className="card-container-patients">
            <DoctorAddPatientComponent onAdd={addPatientWithAccount} />
            {patients.map((patient: IPatient) =>
                patient.id !== updateId ? (
                    <DoctorDisplayPatients
                        key={patient.id}
                        patient={patient}
                        delete={deletePatient}
                        modify={() => setUpdateId(patient.id)}
                    />
                ) : (
                        <DoctorModifyPatientComponent
                            patient={patient}
                            cancel={() => setUpdateId(0)}
                            onModify={modifyPatient}
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