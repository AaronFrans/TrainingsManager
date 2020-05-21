using DomainLibrary.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLibrary.Repositories
{
    public interface ICyclingRepository
    {
        void AddTraining(CyclingSession training);
        void RemoveTraining(int id);
        CyclingSession Find(int id);
        IEnumerable<CyclingSession> FindAll();
        IEnumerable<CyclingSession> Find(DateTime start,DateTime stop);
        CyclingSession[] FindMaxSessions(); //distance + time
        IEnumerable<CyclingSession> FindLatestSessions(int number);

    }
}
