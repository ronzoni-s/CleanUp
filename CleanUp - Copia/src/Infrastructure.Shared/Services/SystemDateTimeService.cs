using ErbertPranzi.Application.Interfaces.Services;
using System;

namespace ErbertPranzi.Infrastructure.Shared.Services
{
    public class SystemDateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
        public long Timestamp => DateTimeOffset.Now.ToUnixTimeSeconds();
    }
}