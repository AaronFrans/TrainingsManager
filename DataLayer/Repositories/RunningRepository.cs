using DomainLibrary.Domain;
using DomainLibrary.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Repositories
{
    public class RunningRepository : IRunningRepository
    {
        private TrainingContext context;

        public RunningRepository(TrainingContext context)
        {
            this.context = context;
        }

        public void AddTraining(RunningSession training)
        {
            context.RunningSessions.Add(training);
        }

        public RunningSession Find(int id)
        {
            return context.RunningSessions.Find(id);
        }
        public IEnumerable<RunningSession> FindAll()
        {
            return context.RunningSessions.OrderBy(s => s.When).AsEnumerable<RunningSession>();
        }

        public IEnumerable<RunningSession> Find(DateTime start, DateTime stop)
        {
            return context.RunningSessions.Where(s => s.When <= stop && s.When >= start).OrderBy(s => s.When).AsEnumerable<RunningSession>();
        }

        public IEnumerable<RunningSession> FindLatestSessions(int number)
        {
            return context.RunningSessions.OrderBy(s => s.When).Take(number).AsEnumerable<RunningSession>();
        }

        public RunningSession[] FindMaxSessions()
        {
            RunningSession[] runs = new RunningSession[2];
            runs[0]=context.RunningSessions.OrderByDescending(s => s.Distance).ThenByDescending(s=>s.AverageSpeed).First();
            runs[1]=context.RunningSessions.OrderByDescending(s => s.AverageSpeed).ThenByDescending(s => s.Distance).First();
            return runs;
        }

        public void RemoveTraining(int id)
        {
            context.RunningSessions.Remove(new RunningSession() { Id = id });
        }
    }
}
