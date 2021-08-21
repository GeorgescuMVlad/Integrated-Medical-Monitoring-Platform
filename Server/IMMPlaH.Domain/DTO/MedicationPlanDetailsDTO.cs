using System;
using System.Collections.Generic;
using System.Text;

namespace IMMPlaH.Domain.DTO
{
    public class MedicationPlanDetailsDTO
    {
        public string MedicationName { get; set; }

        public int Dosage { get; set; }

        public string IntakeInterval { get; set; }

        public string PeriodOfTreatment { get; set; }
    }
}
