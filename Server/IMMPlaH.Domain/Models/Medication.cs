using System;
using System.Collections.Generic;
using System.Text;

namespace IMMPlaH.Domain.Models
{
    public class Medication
    {
        public int Id { set; get; }

        public string Name { set; get; }

        public string SideEffects { set; get; }

        public int Dosage { set; get; }

        public List<MedicationPlan> MedicationPlans { set; get; }
    }
}
