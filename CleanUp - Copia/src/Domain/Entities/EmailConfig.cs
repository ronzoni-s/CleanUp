using ErbertPranzi.Domain.Contracts;
using System;
using System.Collections.Generic;

namespace ErbertPranzi.Domain.Entities
{
    public class EmailConfig : AuditableEntity<int>
    {
        public string Name { get; set; }
        public bool UseSsl { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseAuthentication { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FromEmail { get; set; }
        public DateTime? LastEmailDateTime { get; set; }
    }
}