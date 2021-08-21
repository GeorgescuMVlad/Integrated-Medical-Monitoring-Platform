using System;
using System.Collections.Generic;
using System.Text;
using IMMPlaH.Domain.Models;

namespace IMMPlaH.Services.Abstractions
{
    public interface IConsumerService
    {
        void saveActivity(List<Activities> activities, List<ActivitiesProblems> activitiesProblems);
    }
}
