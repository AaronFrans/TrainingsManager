using DomainLibrary.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLibrary
{
    public interface IUnitOfWork : IDisposable
    {
        ICyclingRepository CyclingTrainings { get; }
        IRunningRepository RunningTrainings { get; }
        int Complete();
    }
}
