using CleanUp.Application.Interfaces.Services;
using System;

namespace CleanUp.Infrastructure.Shared.Services
{
    public class SystemDateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
        public long Timestamp => DateTimeOffset.Now.ToUnixTimeSeconds();
    }
}