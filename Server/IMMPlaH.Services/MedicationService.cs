using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMMPlaH.DataAccess;
using IMMPlaH.Domain.Models;
using IMMPlaH.Services.Abstractions;
using IMMPlaH.Services.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace IMMPlaH.Services
{
    public class MedicationService : IMedicationService
    {
        private readonly AppDbContext _dbContext;

        public MedicationService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Medication> GetMedications()
        {
            return _dbContext.Medications.ToList();
        }

        public void AddMedication(Medication medication)
        {
            try
            {
                if (medication == null)
                {
                    throw new MedicationServiceException("You can't add an empty medication");
                }
                _dbContext.Add(medication);
                _dbContext.SaveChanges();
            }
            catch (Exception e) when (e is DbUpdateException || e is DbUpdateConcurrencyException)
            {
                throw new DatabaseException("Ooops! We have some trouble with the database. Try again later.");
            }
        }

        public void UpdateMedication(int id, Medication medication)
        {
            var medicationUp = _dbContext.Medications.FirstOrDefault(m => m.Id == id);

            medicationUp.Name = medication.Name;
            medicationUp.SideEffects = medication.SideEffects;
            medicationUp.Dosage = medication.Dosage;

            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception e) when (e is DbUpdateException || e is DbUpdateConcurrencyException)
            {
                throw new DatabaseException("Ooops! We have some trouble with the database. Try again later.");
            }
        }

        public void DeleteMedication(int id)
        {
            var medication = _dbContext.Medications.FirstOrDefault(m => m.Id == id);
            _dbContext.Remove(medication);
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception e) when (e is DbUpdateException || e is DbUpdateConcurrencyException)
            {
                 throw new DatabaseException("Ooops! We have some trouble with the database. Try again later.");
            }           
        }

        public int GetNoOfMedications()
        {
            return _dbContext.Medications.Count();
        }
    }
}
