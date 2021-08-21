using System;
using System.Collections.Generic;
using System.Text;
using IMMPlaH.Domain.DTO;

namespace IMMPlaH.Services.Abstractions
{
    public interface IMedicationPlanService
    {
        List<MedicationPlanDetailsDTO> GetPlansOfSelectedPatient(int patientId);

        void AddPlanWithMedications(PlanWithMedicationsDTO planWithMedicationsDTO);
    }
}
