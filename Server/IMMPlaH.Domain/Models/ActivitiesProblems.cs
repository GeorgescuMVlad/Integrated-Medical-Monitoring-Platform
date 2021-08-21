using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace IMMPlaH.Domain.Models
{
    public class ActivitiesProblems
    {
        public int Id { get; set; }

        public int PatientId { get; set; }

        public string Activity { get; set; }

        public string Start { get; set; }

        public string End { get; set; }

        [JsonIgnore]
        public Patient Patient { set; get; }
    }
}
