using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using IMMPlaH.API.SecurityFilter;
using IMMPlaH.Domain.Enums;
using IMMPlaH.Domain.Models;
using IMMPlaH.Services.Abstractions;
using IMMPlaH.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace IMMPlaH.API.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class MedicationController : ControllerBase
    {
        private readonly IMedicationService _medicationService;

        public MedicationController(IMedicationService medicationService)
        {
            _medicationService = medicationService;
        }

        [HttpGet]
        [TokenAuthorization(Role = RoleEnum.Doctor)]
        public ActionResult<List<Medication>> GetMedications()
        {
            return _medicationService.GetMedications();
        }

        [HttpPost]
        [TokenAuthorization(Role = RoleEnum.Doctor)]
        public ActionResult<string> AddMedication(Medication medication)
        {
            try
            {
                _medicationService.AddMedication(medication);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
            return Ok("Medication added!");
        }

        [HttpPut("{id}")]
        [TokenAuthorization(Role = RoleEnum.Doctor)]
        public ActionResult UpdateMedication(int id, Medication medication)
        {
            try
            {
                _medicationService.UpdateMedication(id, medication);
            }
            catch (MedicationServiceException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }

            return Ok("Medication updated!");
        }

        [HttpDelete("{id}")]
        [TokenAuthorization(Role = RoleEnum.Doctor)]
        public ActionResult<string> DeleteMedication(int id)
        {
            try
            {
                _medicationService.DeleteMedication(id);
            }
            catch (CaregiverServiceException e)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, "Something bad happened: " + e.Message);
            }
            catch (DatabaseException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Db exception: " + e.Message);
            }

            return Ok("Medication deleted!");
        }

        [HttpGet("count")]
        [TokenAuthorization(Role = RoleEnum.Doctor)]
        public ActionResult<int> GetNoOfMedications()
        {
            return Ok(_medicationService.GetNoOfMedications());
        }
    }
}
