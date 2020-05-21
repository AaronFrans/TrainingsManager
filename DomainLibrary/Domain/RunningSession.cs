using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLibrary.Domain
{
    public class RunningSession
    {
        public RunningSession()
        {
        }

        public RunningSession(DateTime when, int distance, TimeSpan time, float? averageSpeed, TrainingType trainingType, string comments)
        {
            When = when;
            Distance = distance;
            Time = time;
            if (averageSpeed == null)
            {
                AverageSpeed = (float)((distance/1000) / time.TotalHours);
            }
            else
            {
                AverageSpeed = (float)averageSpeed;
            }
            TrainingType = trainingType;
            Comments = comments;
        }

        public int Id { get; set; }
        public DateTime When { get; set; }
        public int Distance { get; set; } //in meters
        public TimeSpan Time { get; set; }
        public float AverageSpeed { get; set; }
        public TrainingType TrainingType { get; set; }
        public string Comments { get; set; }
        public override string ToString()
        {
            return $"Run : {Id},{When},{Distance},{Time},{AverageSpeed},{TrainingType}";
        }
    }
}
