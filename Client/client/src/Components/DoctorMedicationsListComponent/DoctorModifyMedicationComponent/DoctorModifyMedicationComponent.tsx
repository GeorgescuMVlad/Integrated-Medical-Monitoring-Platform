import React, { useState } from "react";
import Modal from "react-bootstrap/Modal";
import "../DoctorModifyMedicationComponent/doctorModifyMedicationComponent.css";
import AlertModal from "../../AlertModal/AlertModal";
import IMedication from "../../../Interfaces/IMedication";

export interface IModifyMedication {
    id: number;
    name: string;
    sideEffects: string;
    dosage: number;
}

interface IProps {
    medication: IMedication;
    onModify: (medication: IMedication) => void;
    cancel: () => void;
}

export default function DoctorModifyMedicationComponent(props: IProps) {
    const [show, setShow] = useState(true);
    const handleClose = () => setShow(false);
    const [medication, setMedication] = useState<IModifyMedication>(props.medication);
    const [errorMessage, setErrorMessage] = useState<null | string>(null);

    function modifyMedication(property: string, newValue: string | number) {
        let updatedMedication: any = Object.assign({}, medication);
        updatedMedication[property] = newValue;
        setMedication(updatedMedication);
    }

    function save() {
        medication.id = props.medication.id;
        props.onModify(medication);
        handleClose();
    }

    return (
        <>
            <Modal show={show} backdrop="static" onHide={handleClose}>
                <Modal.Body>
                    <p className="title">Edit Medication</p>
                    <p className="medication">Medication name</p>
                    <input
                        className="medication-input"
                        type={"text"}
                        value={medication.name}
                        onChange={(e) => modifyMedication("name", e.target.value)}
                    />
                    <p className="medication">Medication side effects</p>
                    <input
                        className="medication-input"
                        type={"text"}
                        value={medication.sideEffects}
                        onChange={(e) => modifyMedication("sideEffects", e.target.value)}
                    />
                    <p className="medication">Medication dosage</p>
                    <input
                        className="medication-input"
                        type={"text"}
                        value={medication.dosage}
                        onChange={(e) => modifyMedication("dosage", +e.target.value)}
                    />
                </Modal.Body>
                <div>
                    <button
                        className="add-new-medication-buttons save"
                        onClick={save}
                    >
                        Save
                    </button>
                    <button
                        className="add-new-medication-buttons cancel"
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