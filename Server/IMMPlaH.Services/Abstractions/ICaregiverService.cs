using System;
using System.Collections.Generic;
using System.Text;
using IMMPlaH.Domain.DTO;
using IMMPlaH.Domain.Models;

namespace IMMPlaH.Services.Abstractions
{
    public interface ICaregiverService
    {
        List<Caregiver> GetCaregivers();

        List<Patient> GetPatientsCaredBySelectedCaregiver(int CaregiverId);

        void AddCaregiverAndAccount(CaregiverWithAccount caregiverWithAccount);

        void UpdateCaregiver(int id, Caregiver caregiver);

        void DeleteCaregiverAndAccount(int id);

        public int GetNoOfCaregivers();
    }
}
