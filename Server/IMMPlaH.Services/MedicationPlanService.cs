using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMMPlaH.DataAccess;
using IMMPlaH.Domain.DTO;
using IMMPlaH.Domain.Models;
using IMMPlaH.Services.Abstractions;
using IMMPlaH.Services.Exceptions;
using EFCore.BulkExtensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace IMMPlaH.Services
{
    public class MedicationPlanService : IMedicationPlanService
    {
        private readonly AppDbContext _dbContext;

        public MedicationPlanService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<MedicationPlanDetailsDTO> GetPlansOfSelectedPatient(int patientId)
        {
            var plans = _dbContext.Plans.Where(p => p.PatientId == patientId)
                .Select(pl =>  pl.Id)
                .ToList();  
            var medicationPlans = _dbContext.MedicationPlans.Where(mp => plans.Contains(mp.PlanId)).ToList();

            var planDetails = medicationPlans.Select(m => new MedicationPlanDetailsDTO
            {
                MedicationName = _dbContext.Medications.Where(md => md.Id == m.MedicationId).First().Name,
                Dosage = _dbContext.Medications.Where(md => md.Id == m.MedicationId).First().Dosage,
                IntakeInterval = m.IntakeInterval,
                PeriodOfTreatment = m.PeriodOfTreatment
            }).ToList();

            return planDetails;
        }

        public void AddPlanWithMedications(PlanWithMedicationsDTO planWithMedicationsDTO)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (planWithMedicationsDTO == null)
                    {
                        throw new PlanServiceException("You can't add an empty plan");
                    }

                    Plan plan = AddPlan(planWithMedicationsDTO);
                    _dbContext.SaveChanges();
                    int id = plan.Id;

                    AddMedicationPlan(planWithMedicationsDTO.MedicationsWithDetails, id);
                    _dbContext.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception e) when (e is DbUpdateException || e is DbUpdateConcurrencyException)
                {
                    throw new DatabaseException("Ooops! We have some trouble with the database. Try again later.");
                }
            }
        }

        private Plan AddPlan(PlanWithMedicationsDTO planWithMedicationsDTO)
        {
            var plan = new Plan()
            {
                PatientId = planWithMedicationsDTO.PatientId
            };

            _dbContext.Plans.Add(plan);
            return plan;
        }

        private void AddMedicationPlan(List<MedicationWithDetailsDTO> medicationsWithDetails, int PlanId)
        {
            if (medicationsWithDetails == null || !medicationsWithDetails.Any())
            {
                throw new ArgumentNullException("medicationsWithDetails", "Can not insert a plan without medication details!");
            }

            var planMedicationsList = medicationsWithDetails.Select(m => new MedicationPlan
            {
                MedicationId = m.MedicationId,
                PlanId = PlanId,
                IntakeInterval = m.IntakeInterval,
                PeriodOfTreatment = m.PeriodOfTreatment

            }).ToList();
            medicationsWithDetails.ForEach(m =>
            {
                if (_dbContext.MedicationPlans.Find(m.MedicationId, PlanId) != null)
                {
                    throw new DuplicateKeyException("Already exists");
                }
            });

            try
            {
                _dbContext.BulkInsert(planMedicationsList);
            }
            catch (Exception e) when (e is DbUpdateException || e is DbUpdateConcurrencyException || e is SqlException)
            {
                throw new DatabaseException("Ooops! We have some trouble with the database. Try again later.");
            }
        }
    }
}
