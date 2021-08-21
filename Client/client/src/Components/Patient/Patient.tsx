import React, { useEffect, useState, useCallback } from "react";
import { url } from "../../Resources/Hosts";
import { Redirect } from "react-router-dom";
import IPatient from "../../Interfaces/IPatient";
import PatientGetMedicationPlans from "./PatientGetMedicationPlans/PatientGetMedicationPlans";

export default function Patient() {
    let connectedPatientMail = localStorage.getItem('email');

    const [patients, setPatients] = useState<IPatient[]>([]);
    const [isAuthorized, setIsAuthorized] = useState(true);
    const [userId, setUserId] = useState<number>(0);

    function getPatients() {
        fetch(`${url}/patient`, {
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

    function getConnectedPatient(patientMail: string | null) {
        fetch(`${url}/user/${patientMail}`, {
            headers: {
                Authorization: `${localStorage.getItem("accessToken")}`,
            },
        }).then((response) => {
            if (response.status === 200) {
                response.json().then((result) => { setUserId(result.id) });
            } else if (response.status === 403) {
                setIsAuthorized(false);
            }
        });
    }

    useEffect(() => {
        getPatients();
    }, []);

    const getConnectedPatientCallback = useCallback(() => {
        getConnectedPatient(connectedPatientMail)
    }, [connectedPatientMail]);

    useEffect(() => {
        getConnectedPatientCallback();
    }, [getConnectedPatientCallback]);


    return (
        <div>
            {patients.map((patient: IPatient) =>
                patient.userId === userId ?
                    <PatientGetMedicationPlans
                        key={patient.id}
                        patients={patients}
                        connectedPatientMail={connectedPatientMail}
                        userId={patient.id}
                        getConnectedPatient={getConnectedPatient}
                    /> : null
            )}
            {!isAuthorized ? <Redirect to="/unauthorized" /> : null}
        </div>
    );
}