import React, { useState } from "react";
import Modal from "react-bootstrap/Modal";
import "../DoctorModifyPatientComponent/doctorModifyPatientComponent.css";
import AlertModal from "../../AlertModal/AlertModal";
import IPatient from "../../../Interfaces/IPatient";

export interface IModifyPatient {
    id: number;
    name: string;
    birthday: string;
    gender: string;
    address: string;
    medicalRecord: string;
    userId: number;
}

interface IProps {
    patient: IPatient;
    onModify: (patient: IPatient) => void;
    cancel: () => void;
}

export default function DoctorModifyPatientComponent(props: IProps) {
    const [show, setShow] = useState(true);
    const handleClose = () => setShow(false);
    const [patient, setPatient] = useState<IModifyPatient>(props.patient);
    const [errorMessage, setErrorMessage] = useState<null | string>(null);

    function modifyPatient(property: string, newValue: string | number) {
        let updatedPatient: any = Object.assign({}, patient);
        updatedPatient[property] = newValue;
        setPatient(updatedPatient);
    }

    function save() {
        patient.id = props.patient.id;
        patient.userId = props.patient.userId;
        props.onModify(patient);
        handleClose();
    }

    return (
        <>
            <Modal show={show} backdrop="static" onHide={handleClose}>
                <Modal.Body>
                    <p className="title">Edit Patient</p>
                    <p className="patient">Patient name</p>
                    <input
                        className="patient-input"
                        type={"text"}
                        value={patient?.name}
                        onChange={(e) => modifyPatient("name", e.target.value)}
                    />
                    <p className="patient">Patient birthday</p>
                    <input
                        className="patient-input"
                        type={"text"}
                        value={patient?.birthday}
                        onChange={(e) => modifyPatient("birthday", e.target.value)}
                    />
                    <p className="patient">Patient address</p>
                    <input
                        className="patient-input"
                        type={"text"}
                        value={patient?.address}
                        onChange={(e) => modifyPatient("address", e.target.value)}
                    />
                    <p className="patient">Patient gender</p>
                    <input
                        className="patient-input"
                        type={"text"}
                        value={patient?.gender}
                        onChange={(e) => modifyPatient("gender", e.target.value)}
                    />
                    <p className="patient">Patient medical record</p>
                    <input
                        className="patient-input"
                        type={"text"}
                        value={patient?.medicalRecord}
                        onChange={(e) => modifyPatient("medicalRecord", e.target.value)}
                    />
                </Modal.Body>
                <div>
                    <button
                        className="add-new-patient-buttons save"
                        onClick={save}
                    >
                        Save
                    </button>
                    <button
                        className="add-new-patient-buttons cancel"
                        onClick={() => {
                            props.cancel();
                            handleClose();
                        }}
                    >
                        Close
                    </button>
                </div>
            </Modal>
            {errorMessage ? (
                <AlertModal message={errorMessage} closeModal={() => setErrorMessage(null)} title={"Error"} />
            ) : null}
        </>
    );
}