using System;
using System.Collections.Generic;
using System.Text;
using IMMPlaH.Domain.DTO;
using IMMPlaH.Domain.Models;

namespace IMMPlaH.Services.Abstractions
{
    public interface IPatientService
    {
        List<Patient> GetPatients();

        void AddPatientAndAccount(PatientWithAccount patientWithAccount);

        void UpdatePatient(int id, Patient patient);

        void DeletePatientAndAccount(int id);

        public int GetNoOfPatients();
    }
}
