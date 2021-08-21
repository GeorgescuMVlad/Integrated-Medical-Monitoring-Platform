using System;
using System.Collections.Generic;
using System.Text;
using IMMPlaH.Domain.DTO;
using IMMPlaH.Domain.Models;

namespace IMMPlaH.Services.Abstractions
{
    public interface ICaregiverPatientsService
    {
        void AddCaregiverPatients(CaregiverPatientsDTO caregiverPatients);

        void DeletePatients(int CaregiverId, int[] PatientIds);

        List<Patient> GetPatientsNotCaredBySelectedCaregiver(int CaregiverId);
    }
}
