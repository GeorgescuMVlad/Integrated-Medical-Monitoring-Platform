using System;
using System.Collections.Generic;
using System.Text;
using IMMPlaH.Domain.Models;

namespace IMMPlaH.Services.Abstractions
{
    public interface IMedicationService
    {
        List<Medication> GetMedications();

        void AddMedication(Medication medication);

        void UpdateMedication(int id, Medication medication);

        void DeleteMedication(int id);

        public int GetNoOfMedications();
    }
}
