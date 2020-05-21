using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLibrary.Domain
{
    public class Report
    {
        public int TotalSessions { get; set; }
        public int CyclingSessions { get; set; }
        public int RunningSessions { get; set; }
        public TimeSpan TotalTrainingTime { get; set; }
        public int TotalRunningDistance { get; set; }
        public TimeSpan TotalRunningTrainingTime { get; set; }
        public float TotalCyclingDistance { get; set; }
        public TimeSpan TotalCyclingTrainingTime { get; set; }

        public DateTime StartDate { get;  set; }
        public DateTime EndDate { get;  set; }
        public IList<CyclingSession> Rides { get;  set; }
        public IList<RunningSession> Runs { get;  set; }
        public CyclingSession MaxWattSessionCycling { get; set; }
        public CyclingSession MaxDistanceSessionCycling { get; set; }
        public CyclingSession MaxSpeedSessionCycling { get; set; }
        public RunningSession MaxDistanceSessionRunning { get; set; }
        public RunningSession MaxSpeedSessionRunning { get; set; }
        public IList<Tuple<SessionType, Object>> TimeLine { get; set; } = new List<Tuple<SessionType, Object>>();
    }
}
