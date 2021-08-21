import React, { useState, useEffect } from "react";
import Modal from "react-bootstrap/Modal";
import "../DoctorAddCaregiverPatients/doctorAddCaregiverPatients.css";
import AlertModal from "../../AlertModal/AlertModal";
import ICaregiver from "../../../Interfaces/ICaregiver";
import IPatient from "../../../Interfaces/IPatient";
import ICaregiverPatients from "../../../Interfaces/ICaregiverPatients";
import { url } from "../../../Resources/Hosts";
import { Redirect } from "react-router-dom";

interface IProps {
    caregiver: ICaregiver;
    addPatients: (caregiverPatients: ICaregiverPatients) => void;
    cancel: () => void;
}

export default function DoctorAddCaregiverPatients(props: IProps) {
    const [show, setShow] = useState(true);
    const handleClose = () => setShow(false);
    const [caregiverPatients, setCaregiverPatients] = useState<ICaregiverPatients>();
    const [patients, setPatients] = useState<IPatient[]>([]);
    const [chosenPatients, setChosenPatients] = useState<number[]>([]);
    const [errorMessage, setErrorMessage] = useState<null | string>(null);
    const [isAuthorized, setIsAuthorized] = useState(true);

    function getPatientsNotCaredBySelectedCaregiver(caregiverId: number) {
        fetch(`${url}/caregiverPatients/${caregiverId}`, {
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
        getPatientsNotCaredBySelectedCaregiver(props.caregiver.id);
    }, [props.caregiver.id]);

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

        let newCaregiverPatients: any = {
            caregiverId: props.caregiver.id,
            patientIds: finalPatientsIds
        };
        setCaregiverPatients(newCaregiverPatients);
    }

    function save() {
        props.addPatients(caregiverPatients!);
        handleClose();
    }

    return (
        <>
            <Modal show={show} backdrop="static" onHide={handleClose}>
                <Modal.Body>
                    <p className="title">Add Caregiver Associated Patients To Care</p>
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