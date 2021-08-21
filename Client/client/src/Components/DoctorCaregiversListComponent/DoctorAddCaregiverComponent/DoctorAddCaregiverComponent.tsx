import React, { useState } from "react";
import Modal from "react-bootstrap/Modal";
import "../DoctorAddCaregiverComponent/doctorAddCaregiverComponent.css";
import AlertModal from "../../AlertModal/AlertModal";

interface ICreateProps {
    onAdd: (caregiverWithAccount: ICreateCaregiverAndAccount) => void;
}

export interface ICreateCaregiverAndAccount {
    email: string;
    password: string;
    caregiver: ICreateCaregiver | null;
}

export interface ICreateCaregiver {
    name: string;
    birthday: string;
    gender: string;
    address: string;
}

export interface ICreateUser {
    email: string;
    password: string;
}

export default function DoctorAddCaregiverComponent(props: ICreateProps) {
    const [show, setShow] = useState(false);
    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);
    const [caregiver, setCaregiver] = useState<ICreateCaregiver>({
        name: "",
        birthday: "",
        gender: "",
        address: ""
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

    function addNewCaregiver(property: string, newValue: string | number) {
        let newCaregiver: any = Object.assign({}, caregiver);
        newCaregiver[property] = newValue;
        setCaregiver(newCaregiver);
    }

    function resetCaregiverAndAccount() {
        setUser({
            email: "",
            password: ""
        });
        setCaregiver({
            name: "",
            birthday: "",
            gender: "",
            address: ""
        });
    }

    function save() {
        let newCaregiver = caregiver;
        let newCaregiverAndAccount: ICreateCaregiverAndAccount = {
            email: user.email,
            password: user.password,
            caregiver: newCaregiver
        }
        props.onAdd(newCaregiverAndAccount);
        resetCaregiverAndAccount();
        handleClose();
    }

    function cancel() {
        resetCaregiverAndAccount();
        handleClose();
    }

    return (
        <>
            <button className="add-caregiver-button" onClick={handleShow}>
                Add New Caregiver
            </button>
            <Modal show={show} backdrop="static" onHide={handleClose}>
                <Modal.Body>
                    <p className="title">Add new Caregiver</p>
                    <p className="caregiver">Caregiver email</p>
                    <input
                        className="caregiver-input"
                        type={"text"}
                        value={user.email}
                        onChange={(e) => addNewUser("email", e.target.value)}
                    />
                    <p className="caregiver">Caregiver password</p>
                    <input
                        className="caregiver-input"
                        type={"text"}
                        value={user.password}
                        onChange={(e) => addNewUser("password", e.target.value)}
                    />
                    <p className="caregiver">Caregiver name</p>
                    <input
                        className="caregiver-input"
                        type={"text"}
                        value={caregiver?.name}
                        onChange={(e) => addNewCaregiver("name", e.target.value)}
                    />
                    <p className="caregiver">Caregiver birthday</p>
                    <input
                        className="caregiver-input"
                        type={"text"}
                        value={caregiver?.birthday}
                        onChange={(e) => addNewCaregiver("birthday", e.target.value)}
                    />
                    <p className="caregiver">Caregiver address</p>
                    <input
                        className="caregiver-input"
                        type={"text"}
                        value={caregiver?.address}
                        onChange={(e) => addNewCaregiver("address", e.target.value)}
                    />
                    <p className="caregiver">Caregiver gender</p>
                    <input
                        className="caregiver-input"
                        type={"text"}
                        value={caregiver?.gender}
                        onChange={(e) => addNewCaregiver("gender", e.target.value)}
                    />
                </Modal.Body>
                <div>
                    <button className="add-new-caregiver-buttons save" onClick={save}>
                        Save
                    </button>
                    <button className="add-new-caregiver-buttons cancel" onClick={cancel}>
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