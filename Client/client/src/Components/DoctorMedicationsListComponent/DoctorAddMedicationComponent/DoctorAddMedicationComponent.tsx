import React, { useState } from "react";
import Modal from "react-bootstrap/Modal";
import "../DoctorAddMedicationComponent/doctorAddMedicationComponent.css";
import AlertModal from "../../AlertModal/AlertModal";

interface ICreateProps {
    onAdd: (addMedication: ICreateMedication) => void;
}

export interface ICreateMedication {
    name: string;
    sideEffects: string;
    dosage: number;
}

export default function DoctorAddMedicationComponent(props: ICreateProps) {
    const [show, setShow] = useState(false);
    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);
    const [medication, setMedication] = useState<ICreateMedication>({
        name: "",
        sideEffects: "",
        dosage: 0
    });
    const [errorMessage, setErrorMessage] = useState<null | string>(null);

    function addNewMedication(property: string, newValue: string | number) {
        let newMedication: any = Object.assign({}, medication);
        newMedication[property] = newValue;
        setMedication(newMedication);
    }

    function resetMedication() {
        setMedication({
            name: "",
            sideEffects: "",
            dosage: 0
        });
    }

    function save() {
        props.onAdd(medication);
        resetMedication();
        handleClose();
    }

    function cancel() {
        resetMedication();
        handleClose();
    }

    return (
        <>
            <button className="add-medication-button" onClick={handleShow}>
                Add New Medication
            </button>
            <Modal show={show} backdrop="static" onHide={handleClose}>
                <Modal.Body>
                    <p className="title">Add new Medication</p>
                    <p className="medication">Medication name</p>
                    <input
                        className="medication-input"
                        type={"text"}
                        value={medication.name}
                        onChange={(e) => addNewMedication("name", e.target.value)}
                    />
                    <p className="medication">Medication side effects</p>
                    <input
                        className="medication-input"
                        type={"text"}
                        value={medication.sideEffects}
                        onChange={(e) => addNewMedication("sideEffects", e.target.value)}
                    />
                    <p className="medication">Medication dosage</p>
                    <input
                        className="medication-input"
                        type={"text"}
                        value={medication.dosage}
                        onChange={(e) => addNewMedication("dosage", +e.target.value)}
                    />
                </Modal.Body>
                <div>
                    <button className="add-new-medication-buttons save" onClick={save}>
                        Save
                    </button>
                    <button className="add-new-medication-buttons cancel" onClick={cancel}>
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