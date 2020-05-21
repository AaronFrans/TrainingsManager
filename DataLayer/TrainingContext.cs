using DomainLibrary.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer
{
    public class TrainingContext : DbContext
    {
        private string connectionString;

        public TrainingContext()
        {
        }

        public TrainingContext(string db="Production") : base()
        {
            SetConnectionString(db);           
        }
        private void SetConnectionString(string db = "Production")
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", optional: false);

            var configuration = builder.Build();
            switch (db)
            {
                case "Production":
                    connectionString = configuration.GetConnectionString("ProdSQLconnection").ToString();
                    break;
                case "Test":
                    connectionString = configuration.GetConnectionString("TestSQLconnection").ToString();
                    break;
            }
        }

        public DbSet<CyclingSession> CyclingSessions { get; set; }
        public DbSet<RunningSession> RunningSessions { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(connectionString==null)
            {
                SetConnectionString();
            }
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
