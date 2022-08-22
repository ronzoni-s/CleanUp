using System;

namespace CleanUp.Application.Interfaces.Services
{
    public interface IDateTimeService
    {
        DateTime NowUtc { get; }
        long Timestamp { get;  }
    }
}