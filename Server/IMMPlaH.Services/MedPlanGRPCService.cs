using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMMPlaH.Services.Abstractions;
using Grpc.Core;
using GrpcService1;

namespace IMMPlaH.Services
{
    public class MedPlanGRPCService : MedPlanService.MedPlanServiceBase
    {
        private readonly IMedicationPlanService _medicationPlanService;

        public MedPlanGRPCService(IMedicationPlanService medicationPlanService)
        {
            _medicationPlanService = medicationPlanService;
        }

        public override async Task<MedicationPlansDetailsDTO> GetMedPlan(Empty requestData, ServerCallContext context)
        {
            MedicationPlansDetailsDTO plans = new MedicationPlansDetailsDTO();
            var getPlansDb = _medicationPlanService.GetPlansOfSelectedPatient(10);

            var plansAdapted = getPlansDb.Select(m => new MedicationPlanDetailsDTO
            {
                MedicationName = m.MedicationName,
                Dosage = m.Dosage,
                IntakeInterval = m.IntakeInterval,
                PeriodOfTreatment = m.PeriodOfTreatment
            }).ToList();

            plans.Items.AddRange(plansAdapted);

            return await Task.FromResult(plans);
        }
    }
}
