using DomainLibrary.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLibrary.Repositories
{
    public interface IRunningRepository
    {
        void AddTraining(RunningSession training);
        void RemoveTraining(int id);
        RunningSession Find(int id);
        IEnumerable<RunningSession> FindAll();
        IEnumerable<RunningSession> Find(DateTime start, DateTime stop);
        RunningSession[] FindMaxSessions(); //distance + time
        IEnumerable<RunningSession> FindLatestSessions(int number);
        //RunningSession FindHardestSession();
    }
}
