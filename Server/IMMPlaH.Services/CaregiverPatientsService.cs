using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    public class CaregiverPatientsService : ICaregiverPatientsService
    {
        private readonly AppDbContext _dbContext;

        public CaregiverPatientsService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Patient> GetPatientsNotCaredBySelectedCaregiver(int CaregiverId)
        {
            var patientsCaregiver = _dbContext.CaregiverPatients.Where(c => c.CaregiverId == CaregiverId).ToList();
            List<Patient> patients = _dbContext.Patients.ToList();

            if (patientsCaregiver.Count()==0)
            {
                return patients;
            }

            patients.RemoveAll(x => patientsCaregiver.Any(y => y.PatientId == x.Id));
            return patients;
        }

        public void AddCaregiverPatients(CaregiverPatientsDTO caregiverPatients)
        {
            if (caregiverPatients.PatientIds == null || !caregiverPatients.PatientIds.Any())
            {
                throw new ArgumentNullException("patientIds", "Can not insert a caregiver without patients ids!");
            }

            var caregiverPatientsList = caregiverPatients.PatientIds.Select(p => new CaregiverPatients
            {
                PatientId = p,
                CaregiverId = caregiverPatients.CaregiverId

            }).ToList();
            caregiverPatients.PatientIds.ForEach(c =>
            {
                if (_dbContext.CaregiverPatients.Find(c, caregiverPatients.CaregiverId) != null)
                {
                    throw new DuplicateKeyException("Already exists");
                }
            });

            try
            {
                _dbContext.BulkInsert(caregiverPatientsList);
            }
            catch (Exception e) when (e is DbUpdateException || e is DbUpdateConcurrencyException || e is SqlException)
            {
                throw new DatabaseException("Ooops! We have some trouble with the database. Try again later.");
            }
        }

        public void DeletePatients(int CaregiverId, int[] PatientIds)
        {
            var patients = _dbContext.CaregiverPatients.Where(c => c.CaregiverId == CaregiverId);

            foreach (var patient in patients)
            {   
                if (PatientIds.Contains(patient.PatientId))
                {
                    _dbContext.CaregiverPatients.Remove(patient);
                }      
            }
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception e) when (e is DbUpdateException || e is DbUpdateConcurrencyException)
            {
                throw new DatabaseException("Ooops! We have some trouble with the database. Try again later.");
            }
        }
    }
}
