using System;
using System.Collections.Generic;
using System.Text;

namespace IMMPlaH.Domain.DTO
{
    public class PlanWithMedicationsDTO
    {
        public int PatientId { get; set; }

        public List<MedicationWithDetailsDTO> MedicationsWithDetails { get; set; }
    }
}
