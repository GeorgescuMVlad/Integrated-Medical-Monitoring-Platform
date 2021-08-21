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
    public class CaregiverService : ICaregiverService
    {
        private readonly AppDbContext _dbContext;

        public CaregiverService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Caregiver> GetCaregivers()
        {
            return _dbContext.Caregivers.ToList();
        }

        public List<Patient> GetPatientsCaredBySelectedCaregiver(int CaregiverId)
        {
            var patientsCaregiver = _dbContext.CaregiverPatients.Where(c => c.CaregiverId == CaregiverId);
            List<Patient> patients = _dbContext.Patients.ToList();

            patients.RemoveAll(x => !patientsCaregiver.Any(y => y.PatientId == x.Id));

            return patients;
        }

        public void AddCaregiverAndAccount(CaregiverWithAccount caregiverWithAccount)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (caregiverWithAccount == null)
                    {
                        throw new CaregiverServiceException("You can't add an empty caregiver");
                    }

                    User user = AddUser(caregiverWithAccount);
                    _dbContext.SaveChanges();
                    int id = user.Id;

                    AddCaregiver(caregiverWithAccount.Caregiver, id);
                    _dbContext.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception e) when (e is DbUpdateException || e is DbUpdateConcurrencyException)
                {
                    throw new DatabaseException("Ooops! We have some trouble with the database. Try again later.");
                }
            }
        }

        public void UpdateCaregiver(int id, Caregiver caregiver)
        {
            var caregiverUp = _dbContext.Caregivers.FirstOrDefault(c => c.Id == id);

            caregiverUp.Name = caregiver.Name;
            caregiverUp.Birthday = caregiver.Birthday;
            caregiverUp.Gender = caregiver.Gender;
            caregiverUp.Address = caregiver.Address;

            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception e) when (e is DbUpdateException || e is DbUpdateConcurrencyException)
            {
                throw new DatabaseException("Ooops! We have some trouble with the database. Try again later.");
            }
        }

        public void DeleteCaregiverAndAccount(int id)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    DeleteCaregiver(id);
                    DeleteCaregiverAccount(id);
                    _dbContext.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception e) when (e is DbUpdateException || e is DbUpdateConcurrencyException)
                {
                    throw new DatabaseException("Ooops! We have some trouble with the database. Try again later.");
                }
            }
        }

        public int GetNoOfCaregivers()
        {
            return _dbContext.Caregivers.Count();
        }

        private User AddUser(CaregiverWithAccount caregiverWithAccount)
        {
            var user = new User()
            {
                Email = caregiverWithAccount.Email,
                Password = caregiverWithAccount.Password,
                Type = "caregiver"
            };

            _dbContext.User.Add(user);
            return user;
        }
        private void AddCaregiver(Caregiver caregiver, int userId)
        {
            var newCaregiver = new Caregiver()
            {
                Name = caregiver.Name,
                Birthday = caregiver.Birthday,
                Gender = caregiver.Gender,
                Address = caregiver.Address,
                UserId = userId
            };

            _dbContext.Caregivers.Add(newCaregiver);
        }

        private void DeleteCaregiver(int caregiverId)
        {
            var caregiver = _dbContext.Caregivers.FirstOrDefault(c => c.UserId == caregiverId);

            _dbContext.Caregivers.Remove(caregiver);
        }

        private void DeleteCaregiverAccount(int id)
        {
            User user = _dbContext.User.FirstOrDefault(u => u.Id == id);

            _dbContext.User.Remove(user);
        }

    }
}
