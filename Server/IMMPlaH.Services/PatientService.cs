using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMMPlaH.DataAccess;
using IMMPlaH.Domain.DTO;
using IMMPlaH.Domain.Models;
using IMMPlaH.Services.Abstractions;
using IMMPlaH.Services.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace IMMPlaH.Services
{
    public class PatientService : IPatientService
    {
        private readonly AppDbContext _dbContext;

        public PatientService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Patient> GetPatients()
        {
            return _dbContext.Patients.ToList();
        }

        public void AddPatientAndAccount(PatientWithAccount patientWithAccount)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (patientWithAccount == null)
                    {
                        throw new PatientServiceException("You can't add an empty patient");
                    }

                    User user = AddUser(patientWithAccount);
                    _dbContext.SaveChanges();
                    int id = user.Id;
                   
                    AddPatient(patientWithAccount.Patient, id);
                    _dbContext.SaveChanges();
                    
                    transaction.Commit();
                }
                catch (Exception e) when (e is DbUpdateException || e is DbUpdateConcurrencyException)
                {
                    throw new DatabaseException("Ooops! We have some trouble with the database. Try again later.");
                }
            }
        }

        public void UpdatePatient(int id, Patient patient)
        {
            var patientUp = _dbContext.Patients.FirstOrDefault(p => p.Id == id);

            patientUp.Name = patient.Name;
            patientUp.Birthday = patient.Birthday;
            patientUp.Gender = patient.Gender;
            patientUp.Address = patient.Address;
            patientUp.MedicalRecord = patient.MedicalRecord;

            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception e) when (e is DbUpdateException || e is DbUpdateConcurrencyException)
            {
                throw new DatabaseException("Ooops! We have some trouble with the database. Try again later.");
            }
        }

        public void DeletePatientAndAccount(int id)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    DeletePatient(id);
                    DeletePatientAccount(id);
                    _dbContext.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception e) when (e is DbUpdateException || e is DbUpdateConcurrencyException)
                {
                    throw new DatabaseException("Ooops! We have some trouble with the database. Try again later.");
                }
            }
        }

        public int GetNoOfPatients()
        {
            return _dbContext.Patients.Count();
        }

        private User AddUser(PatientWithAccount patientWithAccount)
        {
            var user = new User()
            {
                Email = patientWithAccount.Email,
                Password = patientWithAccount.Password,
                Type = "patient"
            };

            _dbContext.User.Add(user);
            return user;
        }
        private void AddPatient(Patient patient, int userId)
        {
            var newPatient = new Patient()
            {
                Name = patient.Name,
                Birthday = patient.Birthday,
                Gender = patient.Gender,
                Address = patient.Address,
                MedicalRecord = patient.MedicalRecord,
                UserId = userId
            };

            _dbContext.Patients.Add(newPatient);         
        }

        private void DeletePatient(int patientId)
        {
            var patient = _dbContext.Patients.FirstOrDefault(p => p.UserId == patientId);

            _dbContext.Patients.Remove(patient); 
        }

        private void DeletePatientAccount(int id)
        {
            User user = _dbContext.User.FirstOrDefault(u => u.Id == id);

            _dbContext.User.Remove(user);
        }
    }
}
