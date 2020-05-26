using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using DomainLibrary.Domain;
using DataLayer;
using System;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System.Collections.Generic;

namespace BusinessLayerTests
{
    [TestClass]
    public class SessionTests
    {
        [TestMethod]
        public void CycleSes_AverageTimeTest()
        {
            CyclingSession testSession = new CyclingSession(new DateTime(2000, 02, 29), 30, new TimeSpan(3, 30, 0), null, 1, TrainingType.Interval, "This is a test", BikeType.IndoorBike);

            float expected = 30 / 3.5f;

            var result = testSession.AverageSpeed;

            result.Should().BeOfType(typeof(float));
            result.Should().Be(expected);

        }
        [TestMethod]
        public void RunSes_AverageTimeTest()
        {
            RunningSession testSession = new RunningSession(new DateTime(2000, 02, 29), 30000, new TimeSpan(3, 30, 0), null, TrainingType.Interval, "This is a test");

            float expected = (30000 / 1000) / 3.5f;

            var result = testSession.AverageSpeed;

            result.Should().BeOfType(typeof(float));
            result.Should().Be(expected);

        }
    }

    [TestClass]
    public class ManagerTests
    {
        [TestMethod]
        public void AddCycle_ExceptionsCheck()
        {
            TrainingManager tm = new TrainingManager(new UnitOfWork(new TrainingContextTest(false)));

            Action act = () => tm.AddCyclingTraining(new DateTime(2021, 05, 23), 500, new TimeSpan(2, 3, 5), null, 10, TrainingType.Endurance, "comment", BikeType.CityBike);

            act.Should().Throw<DomainException>().WithMessage("Training is in the future");

            act = () => tm.AddCyclingTraining(new DateTime(2020, 05, 20), -1, new TimeSpan(2, 3, 5), 10, 50, TrainingType.Endurance, "comment", BikeType.CityBike);

            act.Should().Throw<DomainException>().WithMessage("Distance invalid value");

            act = () => tm.AddCyclingTraining(new DateTime(2020, 05, 20), null, new TimeSpan(-1), 10, 50, TrainingType.Endurance, "comment", BikeType.CityBike);

            act.Should().Throw<DomainException>().WithMessage("Time invalid value");

            act = () => tm.AddCyclingTraining(new DateTime(2020, 05, 20), null, new TimeSpan(2), 6000, 50, TrainingType.Endurance, "comment", BikeType.CityBike);

            act.Should().Throw<DomainException>().WithMessage("Average speed invalid value");

            act = () => tm.AddCyclingTraining(new DateTime(2020, 05, 20), null, new TimeSpan(2), 10, 80000, TrainingType.Endurance, "comment", BikeType.CityBike);

            act.Should().Throw<DomainException>().WithMessage("Average watt invalid value");

            act = () => tm.AddCyclingTraining(new DateTime(2020, 05, 20), null, new TimeSpan(2), 10, 1, TrainingType.Endurance, "comment", BikeType.CityBike);

            act.Should().NotThrow<Exception>();
        }

        [TestMethod]
        public void AddRun_ExceptionsCheck()
        {
            TrainingManager tm = new TrainingManager(new UnitOfWork(new TrainingContextTest(false)));

            Action act = () => tm.AddRunningTraining(new DateTime(2020, 01, 02), 200, new TimeSpan(1), 30, TrainingType.Endurance, "test");

            act.Should().NotThrow<DomainException>();

            act = () => tm.AddRunningTraining(new DateTime(2021, 1, 02), 200, new TimeSpan(1), 30, TrainingType.Endurance, "test");

            act.Should().Throw<DomainException>().WithMessage("Training is in the future");

            act = () => tm.AddRunningTraining(new DateTime(2020, 1, 02), 500000, new TimeSpan(1), 30, TrainingType.Endurance, "test");

            act.Should().Throw<DomainException>().WithMessage("Distance invalid value");

            act = () => tm.AddRunningTraining(new DateTime(2020, 1, 02), 200, new TimeSpan(-1), 30, TrainingType.Endurance, "test");

            act.Should().Throw<DomainException>().WithMessage("Time invalid value");

            act = () => tm.AddRunningTraining(new DateTime(2020, 1, 02), 200, new TimeSpan(1), 3000, TrainingType.Endurance, "test");

            act.Should().Throw<DomainException>().WithMessage("Average speed invalid value");
        }
    }

    [TestClass]
    public class ManagerReportTests
    {
        [TestMethod]
        public void CycleReportTests()
        {
            TrainingManager tm = new TrainingManager(new UnitOfWork(new TrainingContextTest(false)));

            tm.AddCyclingTraining(new DateTime(2019, 10, 1), 200, new TimeSpan(2, 30, 0), 20, 2, TrainingType.Endurance, "number 1", BikeType.IndoorBike);
            tm.AddCyclingTraining(new DateTime(2019, 10, 2), 300, new TimeSpan(2, 30, 0), 20, 2, TrainingType.Endurance, "number 2", BikeType.IndoorBike);
            tm.AddCyclingTraining(new DateTime(2019, 10, 3), 150, new TimeSpan(2, 0, 0), 20, 2, TrainingType.Endurance, "number 3", BikeType.IndoorBike);
            tm.AddCyclingTraining(new DateTime(2018, 10, 1), 200, new TimeSpan(2, 30, 0), 20, 2, TrainingType.Endurance, "number 4", BikeType.IndoorBike);

            int expectedNumberOfTrainings = 3;
            float expectedDistance = 200;

            Report report = tm.GenerateMonthlyCyclingReport(2019, 10);

            report.CyclingSessions.Should().Be(expectedNumberOfTrainings);

            report.Rides[0].Distance.Should().Be(expectedDistance);
        }

        [TestMethod]
        public void RunReportTests()
        {
            TrainingManager tm = new TrainingManager(new UnitOfWork(new TrainingContextTest(false)));

            tm.AddRunningTraining(new DateTime(2019, 10, 1), 200, new TimeSpan(2, 30, 0), 20, TrainingType.Endurance, "number 1");
            tm.AddRunningTraining(new DateTime(2019, 10, 2), 300, new TimeSpan(2, 30, 0), 20, TrainingType.Endurance, "number 2");
            tm.AddRunningTraining(new DateTime(2019, 10, 3), 150, new TimeSpan(2, 30, 0), 20, TrainingType.Endurance, "number 3");
            tm.AddRunningTraining(new DateTime(2018, 10, 1), 200, new TimeSpan(2, 30, 0), 20, TrainingType.Endurance, "number 4");

            int expectedNumberOfTrainings = 3;
            int expectedDistance = 200;

            Report report = tm.GenerateMonthlyRunningReport(2019, 10);

            report.RunningSessions.Should().Be(expectedNumberOfTrainings);

            report.Runs[0].Distance.Should().Be(expectedDistance);
        }

        [TestMethod]
        public void AllReportTests()
        {
            TrainingManager tm = new TrainingManager(new UnitOfWork(new TrainingContextTest(false)));

            tm.AddRunningTraining(new DateTime(2019, 10, 1), 200, new TimeSpan(2, 30, 0), 20, TrainingType.Endurance, "number 1");
            tm.AddRunningTraining(new DateTime(2019, 10, 2), 300, new TimeSpan(2, 30, 0), 20, TrainingType.Endurance, "number 2");
            tm.AddRunningTraining(new DateTime(2019, 10, 3), 150, new TimeSpan(2, 30, 0), 20, TrainingType.Endurance, "number 3");
            tm.AddRunningTraining(new DateTime(2018, 10, 1), 200, new TimeSpan(2, 30, 0), 20, TrainingType.Endurance, "number 4");

            tm.AddCyclingTraining(new DateTime(2019, 10, 4), 200, new TimeSpan(2, 30, 0), 20, 2, TrainingType.Endurance, "number 1", BikeType.IndoorBike);
            tm.AddCyclingTraining(new DateTime(2019, 10, 5), 300, new TimeSpan(2, 30, 0), 20, 2, TrainingType.Endurance, "number 2", BikeType.IndoorBike);
            tm.AddCyclingTraining(new DateTime(2019, 10, 6), 150, new TimeSpan(2, 0, 0), 20, 2, TrainingType.Endurance, "number 3", BikeType.IndoorBike);
            tm.AddCyclingTraining(new DateTime(2018, 10, 2), 200, new TimeSpan(2, 30, 0), 20, 2, TrainingType.Endurance, "number 4", BikeType.IndoorBike);

            int expectedNumberOfTrainingsPerType = 3;
            int expectedNumberOfTrainingsTotal = 6;
            int expectedDistance = 200;

            Report report = tm.GenerateMonthlyTrainingsReport(2019, 10);

            report.RunningSessions.Should().Be(expectedNumberOfTrainingsPerType);

            report.Runs[0].Distance.Should().Be(expectedDistance);

            report.CyclingSessions.Should().Be(expectedNumberOfTrainingsPerType);

            report.Rides[0].Distance.Should().Be(expectedDistance);

            report.TotalSessions.Should().Be(expectedNumberOfTrainingsTotal);
        }
    }

    [TestClass]
    public class ManagerSessionTests
    {
        [TestMethod]
        public void PreviousRunningSessionTests()
        {
            TrainingManager tm = new TrainingManager(new UnitOfWork(new TrainingContextTest(false)));

            tm.AddRunningTraining(new DateTime(2019, 10, 1), 350, new TimeSpan(2, 50, 0), 25, TrainingType.Interval, "number 1");
            tm.AddRunningTraining(new DateTime(2019, 10, 2), 300, new TimeSpan(2, 30, 0), 20, TrainingType.Endurance, "number 2");
            tm.AddRunningTraining(new DateTime(2019, 10, 3), 150, new TimeSpan(2, 40, 0), 30, TrainingType.Interval, "number 3");
            tm.AddRunningTraining(new DateTime(2018, 10, 1), 200, new TimeSpan(2, 30, 0), 20, TrainingType.Endurance, "number 4");

            DateTime session1Date = new DateTime(2019, 10, 1);
            int session1Distance = 350;
            TimeSpan session1Time = new TimeSpan(2, 50, 0);
            float session1AveSpeed = 25;
            TrainingType session1TrainType = TrainingType.Interval;
            string session1Comment = "number 1";

            DateTime session0Date = new DateTime(2018, 10, 1);
            int session0Distance = 200;
            TimeSpan session0Time = new TimeSpan(2, 30, 0);
            float session0AveSpeed = 20;
            TrainingType session0TrainType = TrainingType.Endurance;
            string session0Comment = "number 4";

            List<RunningSession> runningSessions = tm.GetPreviousRunningSessions(2);

            runningSessions[0].When.Should().Be(session0Date);
            runningSessions[0].Distance.Should().Be(session0Distance);
            runningSessions[0].Time.Should().Be(session0Time);
            runningSessions[0].AverageSpeed.Should().Be(session0AveSpeed);
            runningSessions[0].TrainingType.Should().Be(session0TrainType);
            runningSessions[0].Comments.Should().BeEquivalentTo(session0Comment);

            runningSessions[1].When.Should().Be(session1Date);
            runningSessions[1].Distance.Should().Be(session1Distance);
            runningSessions[1].Time.Should().Be(session1Time);
            runningSessions[1].AverageSpeed.Should().Be(session1AveSpeed);
            runningSessions[1].TrainingType.Should().Be(session1TrainType);
            runningSessions[1].Comments.Should().BeEquivalentTo(session1Comment);

        }
        [TestMethod]
        public void PreviousCyclingSessionTests()
        {
            TrainingManager tm = new TrainingManager(new UnitOfWork(new TrainingContextTest(false)));

            tm.AddCyclingTraining(new DateTime(2019, 10, 1), 350, new TimeSpan(2, 50, 0), 25, 5, TrainingType.Interval, "number 1", BikeType.CityBike);
            tm.AddCyclingTraining(new DateTime(2019, 10, 2), 300, new TimeSpan(2, 30, 0), 20, 2, TrainingType.Endurance, "number 2", BikeType.IndoorBike);
            tm.AddCyclingTraining(new DateTime(2019, 10, 3), 150, new TimeSpan(2, 0, 0), 20, 2, TrainingType.Endurance, "number 3", BikeType.IndoorBike);
            tm.AddCyclingTraining(new DateTime(2018, 10, 1), 200, new TimeSpan(2, 30, 0), 20, 2, TrainingType.Endurance, "number 4", BikeType.IndoorBike);

            DateTime session1Date = new DateTime(2019, 10, 1);
            int session1Distance = 350;
            TimeSpan session1Time = new TimeSpan(2, 50, 0);
            float session1AveSpeed = 25;
            TrainingType session1TrainType = TrainingType.Interval;
            string session1Comment = "number 1";
            int session1Watt = 5;
            BikeType session1BikeType = BikeType.CityBike;

            DateTime session0Date = new DateTime(2018, 10, 1);
            int session0Distance = 200;
            TimeSpan session0Time = new TimeSpan(2, 30, 0);
            float session0AveSpeed = 20;
            TrainingType session0TrainType = TrainingType.Endurance;
            string session0Comment = "number 4";
            int session0Watt = 2;
            BikeType session0BikeType = BikeType.IndoorBike;

            List<CyclingSession> cyclingSessions = tm.GetPreviousCyclingSessions(2);

            cyclingSessions[0].When.Should().Be(session0Date);
            cyclingSessions[0].Distance.Should().Be(session0Distance);
            cyclingSessions[0].Time.Should().Be(session0Time);
            cyclingSessions[0].AverageSpeed.Should().Be(session0AveSpeed);
            cyclingSessions[0].TrainingType.Should().Be(session0TrainType);
            cyclingSessions[0].Comments.Should().BeEquivalentTo(session0Comment);
            cyclingSessions[0].AverageWatt.Should().Be(session0Watt);
            cyclingSessions[0].BikeType.Should().Be(session0BikeType);

            cyclingSessions[1].When.Should().Be(session1Date);
            cyclingSessions[1].Distance.Should().Be(session1Distance);
            cyclingSessions[1].Time.Should().Be(session1Time);
            cyclingSessions[1].AverageSpeed.Should().Be(session1AveSpeed);
            cyclingSessions[1].TrainingType.Should().Be(session1TrainType);
            cyclingSessions[1].Comments.Should().BeEquivalentTo(session1Comment);
            cyclingSessions[1].AverageWatt.Should().Be(session1Watt);
            cyclingSessions[1].BikeType.Should().Be(session1BikeType);
        }
        [TestMethod]
        public void AllRunningSessionTests()
        {
            TrainingManager tm = new TrainingManager(new UnitOfWork(new TrainingContextTest(false)));

            tm.AddRunningTraining(new DateTime(2019, 10, 1), 350, new TimeSpan(2, 50, 0), 25, TrainingType.Interval, "number 1");
            tm.AddRunningTraining(new DateTime(2019, 10, 2), 300, new TimeSpan(2, 30, 0), 20, TrainingType.Endurance, "number 2");
            tm.AddRunningTraining(new DateTime(2019, 10, 3), 150, new TimeSpan(2, 40, 0), 30, TrainingType.Interval, "number 3");
            tm.AddRunningTraining(new DateTime(2018, 10, 1), 200, new TimeSpan(2, 30, 0), 20, TrainingType.Endurance, "number 4");


            DateTime session0Date = new DateTime(2018, 10, 1);
            int session0Distance = 200;
            TimeSpan session0Time = new TimeSpan(2, 30, 0);
            float session0AveSpeed = 20;
            TrainingType session0TrainType = TrainingType.Endurance;
            string session0Comment = "number 4";

            DateTime session3Date = new DateTime(2019, 10, 3);
            int session3Distance = 150;
            TimeSpan session3Time = new TimeSpan(2, 40, 0);
            float session3AveSpeed = 30;
            TrainingType session3TrainType = TrainingType.Interval;
            string session3Comment = "number 3";

            int nrOfSessions = 4;

            List<RunningSession> runningSessions = tm.GetAllRunningSessions();

            runningSessions.Count.Should().Be(nrOfSessions);

            runningSessions[3].When.Should().Be(session3Date);
            runningSessions[3].Distance.Should().Be(session3Distance);
            runningSessions[3].Time.Should().Be(session3Time);
            runningSessions[3].AverageSpeed.Should().Be(session3AveSpeed);
            runningSessions[3].TrainingType.Should().Be(session3TrainType);
            runningSessions[3].Comments.Should().BeEquivalentTo(session3Comment);

            runningSessions[0].When.Should().Be(session0Date);
            runningSessions[0].Distance.Should().Be(session0Distance);
            runningSessions[0].Time.Should().Be(session0Time);
            runningSessions[0].AverageSpeed.Should().Be(session0AveSpeed);
            runningSessions[0].TrainingType.Should().Be(session0TrainType);
            runningSessions[0].Comments.Should().BeEquivalentTo(session0Comment);
        }
        [TestMethod]
        public void AllCyclingSessionTests()
        {
            TrainingManager tm = new TrainingManager(new UnitOfWork(new TrainingContextTest(false)));

            tm.AddCyclingTraining(new DateTime(2019, 10, 1), 350, new TimeSpan(2, 50, 0), 25, 5, TrainingType.Interval, "number 1", BikeType.CityBike);
            tm.AddCyclingTraining(new DateTime(2019, 10, 2), 300, new TimeSpan(2, 30, 0), 20, 2, TrainingType.Endurance, "number 2", BikeType.IndoorBike);
            tm.AddCyclingTraining(new DateTime(2019, 10, 3), 150, new TimeSpan(2, 40, 0), 30, 10, TrainingType.Interval, "number 3",BikeType.IndoorBike);
            tm.AddCyclingTraining(new DateTime(2018, 10, 1), 200, new TimeSpan(2, 30, 0), 20, 2, TrainingType.Endurance, "number 4", BikeType.IndoorBike);

            DateTime session3Date = new DateTime(2019, 10, 3);
            int session3Distance = 150;
            TimeSpan session3Time = new TimeSpan(2, 40, 0);
            float session3AveSpeed = 30;
            TrainingType session3TrainType = TrainingType.Interval;
            string session3Comment = "number 3";
            int session3Watt = 10;
            BikeType session3BikeType = BikeType.IndoorBike;

            DateTime session0Date = new DateTime(2018, 10, 1);
            int session0Distance = 200;
            TimeSpan session0Time = new TimeSpan(2, 30, 0);
            float session0AveSpeed = 20;
            TrainingType session0TrainType = TrainingType.Endurance;
            string session0Comment = "number 4";
            int session0Watt = 2;
            BikeType session0BikeType = BikeType.IndoorBike;

            int nrOfSessions = 4;

            List<CyclingSession> cyclingSessions = tm.GetAllCyclingSessions();

            cyclingSessions.Count.Should().Be(nrOfSessions);

            cyclingSessions[0].When.Should().Be(session0Date);
            cyclingSessions[0].Distance.Should().Be(session0Distance);
            cyclingSessions[0].Time.Should().Be(session0Time);
            cyclingSessions[0].AverageSpeed.Should().Be(session0AveSpeed);
            cyclingSessions[0].TrainingType.Should().Be(session0TrainType);
            cyclingSessions[0].Comments.Should().BeEquivalentTo(session0Comment);
            cyclingSessions[0].AverageWatt.Should().Be(session0Watt);
            cyclingSessions[0].BikeType.Should().Be(session0BikeType);

            cyclingSessions[3].When.Should().Be(session3Date);
            cyclingSessions[3].Distance.Should().Be(session3Distance);
            cyclingSessions[3].Time.Should().Be(session3Time);
            cyclingSessions[3].AverageSpeed.Should().Be(session3AveSpeed);
            cyclingSessions[3].TrainingType.Should().Be(session3TrainType);
            cyclingSessions[3].Comments.Should().BeEquivalentTo(session3Comment);
            cyclingSessions[3].AverageWatt.Should().Be(session3Watt);
            cyclingSessions[3].BikeType.Should().Be(session3BikeType);
        }
    }

}
