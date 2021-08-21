using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace IMMPlaH.Domain.Models
{
    public class MedicationPlan
    {
        public int PlanId { get; set; }

        [JsonIgnore]
        public Plan Plan { get; set; }

        public int MedicationId { get; set; }

        [JsonIgnore]
        public Medication Medication { get; set; }

        public string IntakeInterval { get; set; }

        public string PeriodOfTreatment { get; set; }
    }
}
