import React, { useState } from "react";
import Modal from "react-bootstrap/Modal";
import "../DoctorAddPatientComponent/doctorAddPatientComponent.css";
import AlertModal from "../../AlertModal/AlertModal";

interface ICreateProps {
    onAdd: (patientWithAccount: ICreatePatientAndAccount) => void;
}

export interface ICreatePatientAndAccount {
    email: string;
    password: string;
    patient: ICreatePatient | null;
}

export interface ICreatePatient {
    name: string;
    birthday: string;
    gender: string;
    address: string;
    medicalRecord: string;
}

export interface ICreateUser {
    email: string;
    password: string;
}

export default function DoctorAddPatientComponent(props: ICreateProps) {
    const [show, setShow] = useState(false);
    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);
    const [patient, setPatient] = useState<ICreatePatient>({
        name: "",
        birthday: "",
        gender: "",
        address: "",
        medicalRecord: ""
    });
    const [errorMessage, setErrorMessage] = useState<null | string>(null);
    const [user, setUser] = useState<ICreateUser>({
        email: "",
        password: ""
    });

    function addNewUser(property: string, newValue: string | number) {
        let newUser: any = Object.assign({}, user);
        newUser[property] = newValue;
        setUser(newUser);
    }

    function addNewPatient(property: string, newValue: string | number) {
        let newPatient: any = Object.assign({}, patient);
        newPatient[property] = newValue;
        setPatient(newPatient);
    }

    function resetPatientAndAccount() {
        setUser({
            email: "",
            password: ""
        });
        setPatient({
            name: "",
            birthday: "",
            gender: "",
            address: "",
            medicalRecord: ""
        });
    }

    function save() {
        let newPatient = patient;
        let newPatientAndAccount: ICreatePatientAndAccount = {
            email: user.email,
            password: user.password,
            patient: newPatient
        }
        props.onAdd(newPatientAndAccount);
        resetPatientAndAccount();
        handleClose();
    }

    function cancel() {
        resetPatientAndAccount();
        handleClose();
    }

    return (
        <>
            <button className="add-patient-button" onClick={handleShow}>
                Add New Patient
            </button>
            <Modal show={show} backdrop="static" onHide={handleClose}>
                <Modal.Body>
                    <p className="title">Add new Patient</p>
                    <p className="patient">Patient email</p>
                    <input
                        className="patient-input"
                        type={"text"}
                        value={user.email}
                        onChange={(e) => addNewUser("email", e.target.value)}
                    />
                    <p className="patient">Patient password</p>
                    <input
                        className="patient-input"
                        type={"text"}
                        value={user.password}
                        onChange={(e) => addNewUser("password", e.target.value)}
                    />
                    <p className="patient">Patient name</p>
                    <input
                        className="patient-input"
                        type={"text"}
                        value={patient?.name}
                        onChange={(e) => addNewPatient("name", e.target.value)}
                    />
                    <p className="patient">Patient birthday</p>
                    <input
                        className="patient-input"
                        type={"text"}
                        value={patient?.birthday}
                        onChange={(e) => addNewPatient("birthday", e.target.value)}
                    />
                    <p className="patient">Patient address</p>
                    <input
                        className="patient-input"
                        type={"text"}
                        value={patient?.address}
                        onChange={(e) => addNewPatient("address", e.target.value)}
                    />
                    <p className="patient">Patient gender</p>
                    <input
                        className="patient-input"
                        type={"text"}
                        value={patient?.gender}
                        onChange={(e) => addNewPatient("gender", e.target.value)}
                    />
                    <p className="patient">Patient medical record</p>
                    <input
                        className="patient-input"
                        type={"text"}
                        value={patient?.medicalRecord}
                        onChange={(e) => addNewPatient("medicalRecord", e.target.value)}
                    />
                </Modal.Body>
                <div>
                    <button className="add-new-patient-buttons save" onClick={save}>
                        Save
                    </button>
                    <button className="add-new-patient-buttons cancel" onClick={cancel}>
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