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
    public class CaregiverController : ControllerBase
    {
        private readonly ICaregiverService _caregiverService;

        public CaregiverController(ICaregiverService caregiverService)
        {
            _caregiverService = caregiverService;
        }

        [HttpGet]
        [TokenAuthorization(Role = RoleEnum.DoctorOrCaregiver)]
        public ActionResult<List<Caregiver>> GetCaregivers()
        {
            return _caregiverService.GetCaregivers();
        }

        [HttpGet("{CaregiverId}")]
        [TokenAuthorization(Role = RoleEnum.DoctorOrCaregiver)]
        public ActionResult<List<Patient>> GetPatientsCaredBySelectedCaregiver(int CaregiverId)
        {
            try
            {
                return Ok(_caregiverService.GetPatientsCaredBySelectedCaregiver(CaregiverId));
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [TokenAuthorization(Role = RoleEnum.Doctor)]
        public ActionResult<string> AddCaregiverAndAccount(CaregiverWithAccount caregiverWithAccount)
        {
            try
            {
                _caregiverService.AddCaregiverAndAccount(caregiverWithAccount);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
            return Ok("Caregiver details and his account added!");
        }

        [HttpPut("{id}")]
        [TokenAuthorization(Role = RoleEnum.Doctor)]
        public ActionResult UpdateCaregiver(int id, Caregiver caregiver)
        {
            try
            {
                _caregiverService.UpdateCaregiver(id, caregiver);
            }
            catch (CaregiverServiceException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }

            return Ok("Caregiver updated!");
        }

        [HttpDelete("{id}")]
        [TokenAuthorization(Role = RoleEnum.Doctor)]
        public ActionResult<string> DeleteCaregiverAndAccount(int id)
        {
            try
            {
                _caregiverService.DeleteCaregiverAndAccount(id);
            }
            catch (CaregiverServiceException e)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, "Something bad happened: " + e.Message);
            }
            catch (DatabaseException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Db exception: " + e.Message);
            }

            return Ok("Caregiver and his account Deleted!");
        }

        [HttpGet("count")]
        [TokenAuthorization(Role = RoleEnum.Doctor)]
        public ActionResult<int> GetNoOfCaregivers()
        {
            return Ok(_caregiverService.GetNoOfCaregivers());
        }
    }
}
