using System;
using System.Collections.Generic;
using System.Text;

namespace IMMPlaH.Domain.Models
{
    public class Plan
    {
        public int Id { set; get; }

        public int PatientId { set; get; }

        public Patient Patient { set; get; }

        public List<MedicationPlan> MedicationPlans { set; get; }
    }
}
