using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using IMMPlaH.API.SecurityFilter;
using IMMPlaH.Domain.DTO;
using IMMPlaH.Domain.Enums;
using IMMPlaH.Domain.Models;
using IMMPlaH.Services.Abstractions;
using IMMPlaH.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace IMMPlaH.API.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        [TokenAuthorization(Role = RoleEnum.DoctorOrPatient)]
        public ActionResult<List<Patient>> GetPatients()
        {
            return _patientService.GetPatients();
        }

        [HttpPost]
        [TokenAuthorization(Role = RoleEnum.Doctor)]
        public ActionResult<string> AddPatientAndAccount(PatientWithAccount patientWithAccount)
        {
            try
            {
                _patientService.AddPatientAndAccount(patientWithAccount);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
            return Ok("Patient details and his account added!");
        }

        [HttpPut("{id}")]
        [TokenAuthorization(Role = RoleEnum.Doctor)]
        public ActionResult UpdatePatient(int id, Patient patient)
        {
            try
            {
                _patientService.UpdatePatient(id, patient);
            }
            catch (PatientServiceException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }

            return Ok("Patient updated!");
        }

        [HttpDelete("{id}")]
        [TokenAuthorization(Role = RoleEnum.Doctor)]
        public ActionResult<string> DeletePatientAndAccount(int id)
        {
            try
            {
                _patientService.DeletePatientAndAccount(id);
            }
            catch (PatientServiceException e)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, "Something bad happened: " + e.Message);
            }
            catch (DatabaseException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Db exception: " + e.Message);
            }

            return Ok("Patient and his account Deleted!");
        }

        [HttpGet("count")]
        [TokenAuthorization(Role = RoleEnum.Doctor)]
        public ActionResult<int> GetNoOfPatients()
        {
            return Ok(_patientService.GetNoOfPatients());
        }
    }
}
