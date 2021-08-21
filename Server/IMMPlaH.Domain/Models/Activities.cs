using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace IMMPlaH.Domain.Models
{
    public class Activities
    {
        public int Id { get; set; }

        public int PatientId { get; set; }

        public string Activity { get; set; }

        public long Start { get; set; }

        public long End { get; set; }

        [JsonIgnore]
        public Patient Patient { set; get; }

    }
}
