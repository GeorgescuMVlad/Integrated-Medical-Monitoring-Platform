import React from "react";
import Modal from "react-bootstrap/Modal";
import "../AlertModal/alertModal.css";

interface IAlertMessage {
    message: null | string;
    closeModal: () => void;
    title: null | string;
}

export default function AlertModal(props: IAlertMessage) {
    let modalColor: string = "";
    if (props.title === "Error") {
        modalColor = "modalRed";
    }

    return (
        <>
            <Modal className={"" + modalColor} size="sm" show={true} onHide={() => props.closeModal()}>
                <Modal.Header closeButton>
                    <Modal.Title>{props.title}</Modal.Title>
                </Modal.Header>
                <Modal.Body>{props.message}</Modal.Body>
            </Modal>
        </>
    );
}