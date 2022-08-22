using System;

namespace ErbertPranzi.Application.Interfaces.Services
{
    public interface IDateTimeService
    {
        DateTime NowUtc { get; }
        long Timestamp { get;  }
    }
}