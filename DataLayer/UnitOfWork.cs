using DataLayer.Repositories;
using DomainLibrary;
using DomainLibrary.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer
{
    public class UnitOfWork : IUnitOfWork
    {
        private TrainingContext context;

        public UnitOfWork(TrainingContext context)
        {
            this.context = context;
            CyclingTrainings = new CyclingRepository(context);
            RunningTrainings = new RunningRepository(context);
        }

        public ICyclingRepository CyclingTrainings { get; private set; }

        public IRunningRepository RunningTrainings { get; private set; }

        public int Complete()
        {
            try
            {
                return context.SaveChanges();
            }
            catch(Exception ex)
            //TODO : SqlExceptions
            {
                throw;
            }
        }

        public void Dispose()
        {
            context.Dispose(); ;
        }
    }
}
