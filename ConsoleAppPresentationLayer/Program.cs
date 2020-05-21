using DataLayer;
using DomainLibrary.Domain;
using System;

namespace ConsoleAppPresentationLayer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            TrainingManager m = new TrainingManager(new UnitOfWork(new TrainingContext("Production")));
            //m.AddCyclingTraining(new DateTime(2020, 4, 21, 16, 00, 00), 40, new TimeSpan(1, 20, 00), 30, null, TrainingType.Endurance, null, BikeType.RacingBike);
            //m.AddCyclingTraining(new DateTime(2020, 4, 18, 18, 00, 00), 40, new TimeSpan(1, 42, 00), null, null, TrainingType.Recuperation, null, BikeType.RacingBike);
            //m.AddCyclingTraining(new DateTime(2020, 4, 19, 16, 45, 00), null, new TimeSpan(1, 0, 00), null, 219, TrainingType.Interval, "5x5 min 270", BikeType.IndoorBike);
            //m.AddRunningTraining(new DateTime(2020, 4, 17, 12, 30, 00), 5000, new TimeSpan(0, 27, 17), null, TrainingType.Endurance, null);
            //m.AddRunningTraining(new DateTime(2020, 4, 19, 12, 30, 00), 5000, new TimeSpan(0, 25, 48), null, TrainingType.Endurance, null);
            //m.AddRunningTraining(new DateTime(2020, 3, 17, 11, 0, 00), 5000, new TimeSpan(0, 28, 10), null, TrainingType.Interval, "3x700m");
            //m.AddRunningTraining(new DateTime(2020, 3, 17, 11, 0, 00), 8000, new TimeSpan(0, 42, 10), null, TrainingType.Endurance, null);
            //Report r1=m.GenerateMonthlyCyclingReport(2020, 4);            
            //Report r2=m.GenerateMonthlyRunningReport(2020, 4);
            Console.WriteLine("---------------------------");
            Report r3=m.GenerateMonthlyTrainingsReport(2020, 4);
            foreach(var s in  r3.TimeLine)
            {
                Console.WriteLine($"{s.Item1.ToString()},{s.Item2}");
            }
        }
    }
}
