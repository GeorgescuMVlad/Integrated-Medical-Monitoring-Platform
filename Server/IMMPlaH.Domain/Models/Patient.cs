using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IMMPlaH.Domain.Models
{
    public class Patient
    {
        public int Id { set; get; }

        public string Name { set; get; }

        public string Birthday { set; get; }

        public string Gender { set; get; }

        public string Address { set; get; }

        public string MedicalRecord { set; get; }

        public User User { set; get; }

        public int UserId { set; get; }

        public List<CaregiverPatients> CaregiverPatients { set; get; }

        public List<Plan> Plans { set; get; }

        public List<Activities> Activities { set; get; }

        public List<ActivitiesProblems> ActivitiesProblems { set; get; }
    }
}
