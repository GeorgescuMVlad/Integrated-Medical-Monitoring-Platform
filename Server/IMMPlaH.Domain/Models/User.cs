using System;
using System.Collections.Generic;
using System.Text;

namespace IMMPlaH.Domain.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Type { get; set; }

        public Patient Patient { get; set; }

        public Caregiver Caregiver { get; set; }
    }
}
