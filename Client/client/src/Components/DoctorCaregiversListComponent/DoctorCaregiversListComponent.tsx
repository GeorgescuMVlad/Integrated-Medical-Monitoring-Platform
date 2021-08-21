import React, { useEffect, useState } from "react";
import "../DoctorCaregiversListComponent/doctorCaregiversListComponent.css";
import { url } from "../../Resources/Hosts";
import { Redirect } from "react-router-dom";
import AlertModal from "../AlertModal/AlertModal";
import ICaregiver from "../../Interfaces/ICaregiver";
import DoctorAddCaregiverComponent, { ICreateCaregiverAndAccount } from "./DoctorAddCaregiverComponent/DoctorAddCaregiverComponent";
import DoctorDisplayCaregivers from "./DoctorDisplayCaregivers/DoctorDisplayCaregivers";
import DoctorModifyCaregiverComponent from "./DoctorModifyCaregiverComponent/DoctorModifyCaregiverComponent";
import DoctorAddCaregiverPatients from "./DoctorAddCaregiverPatients/DoctorAddCaregiverPatients";
import ICaregiverPatients from "../../Interfaces/ICaregiverPatients";
import DoctorDeleteCaregiverPatients from "./DoctorDeleteCaregiverPatients/DoctorDeleteCaregiverPatients";

export default function DoctorCaregiversListComponent() {
    //let connectedDoctorMail = localStorage.getItem('email');
    const [caregivers, setCaregivers] = useState<ICaregiver[]>([]);
    const [errorMessage, setErrorMessage] = useState<null | string>(null);
    const [updateId, setUpdateId] = useState(0);
    const [addPatients, setAddPatients] = useState(0);
    const [isAuthorized, setIsAuthorized] = useState(true);

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

    function addCaregiverWithAccount(caregiverAndAccount: ICreateCaregiverAndAccount) {
        fetch(`${url}/caregiver`, {
            method: "POST",
            body: JSON.stringify(caregiverAndAccount),
            headers: {
                Authorization: `${localStorage.getItem("accessToken")}`,
                "Content-Type": "application/json",
            },
        }).then((response) => {
            if (response.status === 200) {
                setErrorMessage("");
                getCaregivers();
            } else {
                setErrorMessage("Ooops! We have some trouble with the server. Try again later.");
            }
        });
    }

    function modifyCaregiver(caregiver: ICaregiver) {
        fetch(`${url}/caregiver/${caregiver.id}`, {
            method: "PUT",
            body: JSON.stringify(caregiver),
            headers: {
                Authorization: `${localStorage.getItem("accessToken")}`,
                "Content-Type": "application/json",
            },
        }).then((response) => {
            if (response.status === 200) {
                setErrorMessage("");
                getCaregivers();
                setUpdateId(0);
            } else {
                setErrorMessage("Ooops! We have some trouble with the server. Try again later.");
            }
        });
    }

    function deleteCaregiver(id: number) {
        fetch(`${url}/caregiver/${id}`, {
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
                getCaregivers();
            }
        });
    }

    function addCaregiverPatients(caregiverPatients: ICaregiverPatients) {
        fetch(`${url}/caregiverPatients`, {
            method: "POST",
            body: JSON.stringify(caregiverPatients),
            headers: {
                Authorization: `${localStorage.getItem("accessToken")}`,
                "Content-Type": "application/json",
            },
        }).then((response) => {
            if (response.status === 200) {
                setErrorMessage("");
                getCaregivers();
                setUpdateId(0);
            } else {
                setErrorMessage("Ooops! We have some trouble with the server. Try again later.");
            }
        });
    }

    function deleteCaregiverPatients(id: number, patientsList: string) {
        fetch(`${url}/caregiverPatients/${id}?` + patientsList, {
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
                getCaregivers();
                setUpdateId(0);
            }
        });
    }

    useEffect(() => {
        getCaregivers();
    }, []);

    return (
        <div className="card-container-caregivers">
            <DoctorAddCaregiverComponent onAdd={addCaregiverWithAccount} />
            {caregivers.map((caregiver: ICaregiver) =>
                updateId === 0 ? (
                    <DoctorDisplayCaregivers
                        key={caregiver.id}
                        caregiver={caregiver}
                        delete={deleteCaregiver}
                        modify={() => setUpdateId(caregiver.id)}
                        addPatients={() => { setUpdateId(caregiver.id); setAddPatients(1) }}
                        deletePatients={() => { setUpdateId(caregiver.id); setAddPatients(2) }}
                    />
                ) : (
                        addPatients === 0 && updateId === caregiver.id ? (
                            <DoctorModifyCaregiverComponent
                                caregiver={caregiver}
                                cancel={() => setUpdateId(0)}
                                onModify={modifyCaregiver}
                            />
                        ) : addPatients === 1 && updateId === caregiver.id ? (
                            <DoctorAddCaregiverPatients
                                caregiver={caregiver}
                                cancel={() => { setUpdateId(0); setAddPatients(0) }}
                                addPatients={addCaregiverPatients}
                            />
                        ) : addPatients === 2 && updateId === caregiver.id ? (
                            <DoctorDeleteCaregiverPatients
                                caregiver={caregiver}
                                cancel={() => { setUpdateId(0); setAddPatients(0) }}
                                deletePatients={deleteCaregiverPatients}
                            />
                        ) : null
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