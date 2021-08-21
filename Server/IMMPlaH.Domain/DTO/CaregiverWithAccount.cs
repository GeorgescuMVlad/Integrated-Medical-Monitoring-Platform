using System;
using System.Collections.Generic;
using System.Text;
using IMMPlaH.Domain.Models;

namespace IMMPlaH.Domain.DTO
{
    public class CaregiverWithAccount
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public Caregiver Caregiver { get; set; }
    }
}
