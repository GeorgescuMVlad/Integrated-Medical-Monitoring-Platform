using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using IMMPlaH.API.SecurityFilter;
using IMMPlaH.Domain.DTO;
using IMMPlaH.Domain.Enums;
using IMMPlaH.Services.Abstractions;
using IMMPlaH.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace IMMPlaH.API.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class MedicationPlanController : ControllerBase
    {
        private readonly IMedicationPlanService _medicationPlanService;

        public MedicationPlanController(IMedicationPlanService medicationPlanService)
        {
            _medicationPlanService = medicationPlanService;
        }

        [HttpGet("{patientId}")]
        //[TokenAuthorization(Role = RoleEnum.Doctor)]
        public ActionResult<List<MedicationPlanDetailsDTO>> GetPlansOfSelectedPatient(int patientId)
        {
            try
            {
                return Ok(_medicationPlanService.GetPlansOfSelectedPatient(patientId));
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost]
        //[TokenAuthorization(Role = RoleEnum.Doctor)]
        public ActionResult AddCaregiverPatients(PlanWithMedicationsDTO planWithMedicationsDTO)
        {
            try
            {
                _medicationPlanService.AddPlanWithMedications(planWithMedicationsDTO);
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
            return Ok("Plan with medications added successfully!");
        }
    }
}
