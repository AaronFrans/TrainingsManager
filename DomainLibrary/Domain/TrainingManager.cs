using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainLibrary.Domain
{
    public class TrainingManager
    {
        private IUnitOfWork uow;

        public TrainingManager(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public void AddCyclingTraining(DateTime when,float? distance,TimeSpan time,float? averageSpeed,int? averageWatt,
            TrainingType trainingType,string comments,BikeType bikeType)
        {
            if (when > DateTime.Now) throw new DomainException("Training is in the future");
            if (distance != null) if ((distance <= 0) || (distance>500)) throw new DomainException("Distance invalid value");
            if ((time.Ticks <= 0) || (time.TotalHours>20)) throw new DomainException("Time invalid value");
            if (averageSpeed != null) if ((averageSpeed <= 0) || (averageSpeed > 60)) throw new DomainException("Average speed invalid value");
            if (averageWatt != null) if ((averageWatt <= 0) || (averageWatt > 800)) throw new DomainException("Average watt invalid value");
            uow.CyclingTrainings.AddTraining(new CyclingSession(when,distance,time, averageSpeed, averageWatt,trainingType,comments,bikeType));
            uow.Complete();
        }
        public Report GenerateMonthlyCyclingReport(int year,int month)
        {
            Report r = new Report();
            r.EndDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            r.StartDate = new DateTime(year, month, 1);

            var s = uow.CyclingTrainings.FindMaxSessions();
            r.MaxDistanceSessionCycling = s[0];
            r.MaxSpeedSessionCycling = s[1];
            r.MaxWattSessionCycling = s[2];

            r.Rides = uow.CyclingTrainings.Find(r.StartDate, r.EndDate).ToList();
            r.CyclingSessions = r.Rides.Count();
            r.TotalSessions = r.CyclingSessions;
            r.TotalCyclingDistance = r.Rides.Where(x=>x.Distance!=null).Sum(x => (float)x.Distance);
            r.TotalCyclingTrainingTime = TimeSpan.FromTicks(r.Rides.Sum(x => x.Time.Ticks));
            r.TotalTrainingTime = r.TotalCyclingTrainingTime;
            foreach (var rs in r.Rides)
                r.TimeLine.Add(Tuple.Create(SessionType.Cycling, (Object)rs));
            return r;
        }
        public void AddRunningTraining(DateTime when, int distance, TimeSpan time, float? averageSpeed, TrainingType trainingType, string comments)
        {
            if (when > DateTime.Now) throw new DomainException("Training is in the future");
            if ((distance <= 0) || (distance > 50000)) throw new DomainException("Distance invalid value");
            if ((time.Ticks <= 0) || (time.TotalHours > 20)) throw new DomainException("Time invalid value");
            if (averageSpeed != null) if ((averageSpeed <= 0) || (averageSpeed > 30)) throw new DomainException("Average speed invalid value");
            uow.RunningTrainings.AddTraining(new RunningSession(when,distance,time,averageSpeed,trainingType,comments));
            uow.Complete();
        }
        public Report GenerateMonthlyRunningReport(int year, int month)
        {
            Report r = new Report();
            r.EndDate= new DateTime(year, month, DateTime.DaysInMonth(year, month));
            r.StartDate = new DateTime(year, month, 1);

            var s = uow.RunningTrainings.FindMaxSessions();
            r.MaxDistanceSessionRunning = s[0];
            r.MaxSpeedSessionRunning = s[1];

            r.Runs = uow.RunningTrainings.Find(r.StartDate, r.EndDate).ToList();
            r.RunningSessions = r.Runs.Count();
            r.TotalSessions = r.RunningSessions;
            r.TotalRunningDistance = r.Runs.Sum(x => x.Distance);
            r.TotalRunningTrainingTime = TimeSpan.FromTicks(r.Runs.Sum(x => x.Time.Ticks));
            r.TotalTrainingTime = r.TotalRunningTrainingTime;
            foreach (var rs in r.Runs) 
                r.TimeLine.Add(Tuple.Create(SessionType.Running,(Object) rs));
            return r;
        }
        public void RemoveTrainings(List<int> cyclingSessionIds,List<int> runningSessionIds)
        {
            foreach(int id in cyclingSessionIds)
            {
                uow.CyclingTrainings.RemoveTraining(id);
            }
            foreach (int id in runningSessionIds)
            {
                uow.RunningTrainings.RemoveTraining(id);
            }
            uow.Complete();
        }
        public Report GenerateMonthlyTrainingsReport(int year,int month)
        {
            Report r = new Report();
            r.EndDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            r.StartDate = new DateTime(year, month, 1);

            var sc = uow.CyclingTrainings.FindMaxSessions();
            r.MaxDistanceSessionCycling = sc[0];
            r.MaxSpeedSessionCycling = sc[1];
            r.MaxWattSessionCycling = sc[2];

            r.Rides = uow.CyclingTrainings.Find(r.StartDate, r.EndDate).ToList();
            r.CyclingSessions = r.Rides.Count();
           
            r.TotalCyclingDistance = r.Rides.Where(x => x.Distance != null).Sum(x => (float)x.Distance);
            r.TotalCyclingTrainingTime = TimeSpan.FromTicks(r.Rides.Sum(x => x.Time.Ticks));
            
            var sr = uow.RunningTrainings.FindMaxSessions();
            r.MaxDistanceSessionRunning = sr[0];
            r.MaxSpeedSessionRunning = sr[1];

            r.Runs = uow.RunningTrainings.Find(r.StartDate, r.EndDate).ToList();
            r.RunningSessions = r.Runs.Count();

            r.TotalRunningDistance = r.Runs.Sum(x => x.Distance);
            r.TotalRunningTrainingTime = TimeSpan.FromTicks(r.Runs.Sum(x => x.Time.Ticks));

            r.TotalSessions = r.CyclingSessions+ r.RunningSessions ;
            r.TotalTrainingTime = r.TotalCyclingTrainingTime + r.TotalRunningTrainingTime;

            SortedList<DateTime,Object> list = new SortedList<DateTime, object>();
            foreach(RunningSession s in r.Runs)
            {
                list.Add(s.When, s);
            }
            foreach (CyclingSession s in r.Rides)
            {
                list.Add(s.When, s);
            }
            foreach (var s in list) 
                r.TimeLine.Add(Tuple.Create(s.Value is CyclingSession ? SessionType.Cycling : SessionType.Running,s.Value));
            return r;
        }
        public List<RunningSession> GetPreviousRunningSessions(int count)
        {
            return uow.RunningTrainings.FindLatestSessions(count).ToList();
        }
        public List<CyclingSession> GetPreviousCyclingSessions(int count)
        {
            return uow.CyclingTrainings.FindLatestSessions(count).ToList();
        }
        public List<RunningSession> GetAllRunningSessions()
        {
            return uow.RunningTrainings.FindAll().ToList();
        }
        public List<CyclingSession> GetAllCyclingSessions()
        {
            return uow.CyclingTrainings.FindAll().ToList();
        }
    }
}
