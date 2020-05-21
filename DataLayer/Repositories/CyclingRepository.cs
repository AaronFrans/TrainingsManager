using DomainLibrary.Domain;
using DomainLibrary.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DataLayer.Repositories
{
    public class CyclingRepository : ICyclingRepository
    {
        private TrainingContext context;

        public CyclingRepository(TrainingContext context)
        {
            this.context = context;
        }

        public void AddTraining(CyclingSession training)
        {
            context.CyclingSessions.Add(training);
        }

        public CyclingSession Find(int id)
        {
            return context.CyclingSessions.Find(id);
        }
        public IEnumerable<CyclingSession> FindAll()
        {
            return context.CyclingSessions.OrderBy(s => s.When).AsEnumerable<CyclingSession>();
        }

        public IEnumerable<CyclingSession> Find(DateTime start, DateTime stop)
        {
            return context.CyclingSessions.Where(s=>s.When<=stop && s.When>=start).OrderBy(s=>s.When).AsEnumerable<CyclingSession>();
        }

        public IEnumerable<CyclingSession> FindLatestSessions(int number)
        {
            return context.CyclingSessions.OrderBy(s => s.When).Take(number).AsEnumerable<CyclingSession>();
        }

        public CyclingSession[] FindMaxSessions()
        {
            CyclingSession[] rides = new CyclingSession[3];
            rides[0]=context.CyclingSessions.OrderByDescending(s => s.Distance).ThenByDescending(s => s.AverageSpeed).First();
            rides[1]=context.CyclingSessions.OrderByDescending(s => s.AverageSpeed).ThenByDescending(s => s.Distance).First();
            rides[2]=context.CyclingSessions.OrderByDescending(s => s.AverageWatt).ThenByDescending(s => s.Time).First();
            return rides;
        }

        public void RemoveTraining(int id)
        {
            context.CyclingSessions.Remove(new CyclingSession() { Id = id });
        }
    }
}
