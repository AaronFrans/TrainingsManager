using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLibrary.Domain
{
    public class CyclingSession
    {
        public CyclingSession()
        {
        }

        public CyclingSession(DateTime when, float? distance, TimeSpan time, float? averageSpeed, int? averageWatt, TrainingType trainingType, string comments, BikeType bikeType)
        {
            When = when;
            Distance = distance;
            Time = time;
            if ((averageSpeed == null) && (distance!=null))
            {
                AverageSpeed = (float) (distance / time.TotalHours);
            }
            else
            {
                AverageSpeed = averageSpeed;
            }
            AverageWatt = averageWatt;
            TrainingType = trainingType;
            Comments = comments;
            BikeType = bikeType;
        }

        public int Id { get; set; }
        public DateTime When { get; set; }
        public float? Distance { get; set; } //in km
        public TimeSpan Time { get; set; }
        public float? AverageSpeed { get; set; }
        public int? AverageWatt { get; set; }
        public TrainingType TrainingType { get; set; }
        public string Comments { get; set; }
        public BikeType BikeType { get; set; }
        public override string ToString()
        {
            return $"Ride : {Id},{When},{Distance},{Time},{AverageSpeed},{AverageWatt},{TrainingType},{BikeType}";
        }
    }
}
