using System;
using System.Collections.Generic;
using System.Text;

namespace IMMPlaH.Domain.DTO
{
    public class CaregiverPatientsDTO
    {
        public int CaregiverId { get; set; }

        public List<int> PatientIds { get; set; }
    }
}
