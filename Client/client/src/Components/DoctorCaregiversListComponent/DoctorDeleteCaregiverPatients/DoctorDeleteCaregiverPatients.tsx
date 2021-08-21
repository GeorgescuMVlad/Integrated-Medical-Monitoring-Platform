import React, { useState, useEffect } from "react";
import Modal from "react-bootstrap/Modal";
import "../DoctorDeleteCaregiverPatients/doctorDeleteCaregiverPatients.css";
import AlertModal from "../../AlertModal/AlertModal";
import ICaregiver from "../../../Interfaces/ICaregiver";
import IPatient from "../../../Interfaces/IPatient";
import { url } from "../../../Resources/Hosts";
import { Redirect } from "react-router-dom";

interface IProps {
    caregiver: ICaregiver;
    deletePatients: (id: number, patientsList: string) => void;
    cancel: () => void;
}

export default function DoctorDeleteCaregiverPatients(props: IProps) {
    const [show, setShow] = useState(true);
    const handleClose = () => setShow(false);
    const [patients, setPatients] = useState<IPatient[]>([]);
    const [chosenPatients, setChosenPatients] = useState<number[]>([]);
    const [errorMessage, setErrorMessage] = useState<null | string>(null);
    const [isAuthorized, setIsAuthorized] = useState(true);

    function getPatientsCaredBySelectedCaregiver(caregiverId: number) {
        fetch(`${url}/caregiver/${caregiverId}`, {
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
        getPatientsCaredBySelectedCaregiver(props.caregiver.id);
    }, [props.caregiver.id]);

    function formUrl() {
        let urlQuerry: string = "";
        for (var i = 0; i < chosenPatients.length; i++) {
            if (i === 0) {
                urlQuerry += "PatientIds=" + chosenPatients[i];
            }
            else if (chosenPatients[i]) {
                urlQuerry += "&PatientIds=" + chosenPatients[i];
            }
        }
        return urlQuerry;
    }

    function modifyCaregiverPatients(property: IPatient, newValue: string | boolean, index: number) {
        if (newValue === true) {
            chosenPatients[index] = property.id;
            setChosenPatients(chosenPatients);
        }
        else {
            chosenPatients[index] = 0;
            setChosenPatients(chosenPatients);
        }
        const finalPatientsIds = chosenPatients.filter(item => item > 0);
        setChosenPatients(finalPatientsIds);
    }

    function save() {
        let urlQuerry: string = formUrl();
        props.deletePatients(props.caregiver.id, urlQuerry);
        handleClose();
    }

    return (
        <>
            <Modal show={show} backdrop="static" onHide={handleClose}>
                <Modal.Body>
                    <p className="title">Delete Caregiver Associated Patients To Care</p>
                    {
                        patients.map((patient: IPatient, index: number) => (
                            <div key={patient.id} className={"patients-selected"}>
                                <input type={"checkbox"}
                                    name="Patient"
                                    onChange={(e) => modifyCaregiverPatients(patient, e.target.checked, index)}
                                    className={"radio-checkbox-round"} />
                                {patient.name}
                            </div>
                        ))}
                </Modal.Body>
                <div>
                    <button
                        className="add-new-caregiver-buttons save"
                        onClick={save}
                    >
                        Save
                    </button>
                    <button
                        className="add-new-caregiver-buttons cancel"
                        onClick={() => {
                            props.cancel();
                            handleClose();
                        }}
                    >
                        Close
                    </button>
                    {!isAuthorized ? <Redirect to="/unauthorized" /> : null}
                </div>
            </Modal>
            {
                errorMessage ? (
                    <AlertModal message={errorMessage} closeModal={() => setErrorMessage(null)} title={"Error"} />
                ) : null
            }
        </>
    );
}