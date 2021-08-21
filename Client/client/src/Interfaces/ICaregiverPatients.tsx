import IPatient from "./IPatient";

export default interface ICaregiverPatients {
    caregiverId: number;
    patientIds: number[];
}