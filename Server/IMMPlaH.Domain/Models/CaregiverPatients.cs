using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace IMMPlaH.Domain.Models
{
    public class CaregiverPatients
    {
        public int CaregiverId { get; set; }

        [JsonIgnore]
        public Caregiver Caregiver { get; set; }

        public int PatientId { get; set; }

        [JsonIgnore]
        public Patient Patient { get; set; }
    }
}
