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
    public class CaregiverPatientsController : ControllerBase
    {
        private readonly ICaregiverPatientsService _caregiverPatientsService;

        public CaregiverPatientsController(ICaregiverPatientsService caregiverPatientsService)
        {
            _caregiverPatientsService = caregiverPatientsService;
        }

        [HttpPost]
        [TokenAuthorization(Role = RoleEnum.Doctor)]
        public ActionResult AddCaregiverPatients(CaregiverPatientsDTO caregiverPatientsDTO)
        {
            try
            {
                _caregiverPatientsService.AddCaregiverPatients(caregiverPatientsDTO);
            }
            catch (Exception e) when (e is DatabaseException)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
            catch (Exception e) when (e is ArgumentNullException)
            {
                return StatusCode((int)HttpStatusCode.UnprocessableEntity, e.Message);
            }
            catch (Exception e) when (e is DuplicateKeyException)
            {
                return StatusCode((int)HttpStatusCode.Conflict, e.Message);
            }
            return Ok("Caregiver Patients added successfully!");
        }

        [HttpDelete("{CaregiverId}")]
        [TokenAuthorization(Role = RoleEnum.Doctor)]
        public ActionResult<string> DeleteCaregiverPatients(int CaregiverId, [FromQuery] int[] PatientIds)
        {
            try
            {
                _caregiverPatientsService.DeletePatients(CaregiverId, PatientIds);
            }
            catch (CaregiverServiceException e)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, "Something bad happened: " + e.Message);
            }
            catch (DatabaseException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Db exception: " + e.Message);
            }

            return Ok("Caregiver's patients deleted!");
        }

        [HttpGet("{CaregiverId}")]
        [TokenAuthorization(Role = RoleEnum.Doctor)]
        public ActionResult<List<Patient>> GetPatientsNotCaredBySelectedCaregiver(int CaregiverId)
        {
            try
            {
                return Ok(_caregiverPatientsService.GetPatientsNotCaredBySelectedCaregiver(CaregiverId));
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
