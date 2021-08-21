using System;
using System.Collections.Generic;
using System.Text;

namespace IMMPlaH.Domain.DTO
{
    public class MedicationWithDetailsDTO
    {
        public int MedicationId { get; set; }

        public string IntakeInterval { get; set; }

        public string PeriodOfTreatment { get; set; }
    }
}
