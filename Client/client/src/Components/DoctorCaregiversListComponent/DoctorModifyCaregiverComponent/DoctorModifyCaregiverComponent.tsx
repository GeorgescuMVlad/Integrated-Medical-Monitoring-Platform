import React, { useState } from "react";
import Modal from "react-bootstrap/Modal";
import "../DoctorModifyCaregiverComponent/doctorModifyCaregiverComponent.css";
import AlertModal from "../../AlertModal/AlertModal";
import ICaregiver from "../../../Interfaces/ICaregiver";

export interface IModifyCaregiver {
    id: number;
    name: string;
    birthday: string;
    gender: string;
    address: string;
    userId: number;
}

interface IProps {
    caregiver: ICaregiver;
    onModify: (caregiver: ICaregiver) => void;
    cancel: () => void;
}

export default function DoctorModifyCaregiverComponent(props: IProps) {
    const [show, setShow] = useState(true);
    const handleClose = () => setShow(false);
    const [caregiver, setCaregiver] = useState<IModifyCaregiver>(props.caregiver);
    const [errorMessage, setErrorMessage] = useState<null | string>(null);

    function modifyCaregiver(property: string, newValue: string | number) {
        let updatedCaregiver: any = Object.assign({}, caregiver);
        updatedCaregiver[property] = newValue;
        setCaregiver(updatedCaregiver);
    }

    function save() {
        caregiver.id = props.caregiver.id;
        caregiver.userId = props.caregiver.userId;
        props.onModify(caregiver);
        handleClose();
    }

    return (
        <>
            <Modal show={show} backdrop="static" onHide={handleClose}>
                <Modal.Body>
                    <p className="title">Edit Caregiver</p>
                    <p className="caregiver">Caregiver name</p>
                    <input
                        className="caregiver-input"
                        type={"text"}
                        value={caregiver?.name}
                        onChange={(e) => modifyCaregiver("name", e.target.value)}
                    />
                    <p className="caregiver">Caregiver birthday</p>
                    <input
                        className="caregiver-input"
                        type={"text"}
                        value={caregiver?.birthday}
                        onChange={(e) => modifyCaregiver("birthday", e.target.value)}
                    />
                    <p className="caregiver">Caregiver address</p>
                    <input
                        className="caregiver-input"
                        type={"text"}
                        value={caregiver?.address}
                        onChange={(e) => modifyCaregiver("address", e.target.value)}
                    />
                    <p className="caregiver">Caregiver gender</p>
                    <input
                        className="caregiver-input"
                        type={"text"}
                        value={caregiver?.gender}
                        onChange={(e) => modifyCaregiver("gender", e.target.value)}
                    />
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
                </div>
            </Modal>
            {errorMessage ? (
                <AlertModal message={errorMessage} closeModal={() => setErrorMessage(null)} title={"Error"} />
            ) : null}
        </>
    );
}