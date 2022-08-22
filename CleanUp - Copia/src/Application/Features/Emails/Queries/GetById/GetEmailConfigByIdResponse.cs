using ErbertPranzi.Application.Features.Products.Queries.GetAllPaged;
using System;
using System.Collections.Generic;

namespace ErbertPranzi.Application.Features.Emails.Queries.GetById
{
    public class GetEmailConfigByIdResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool UseSsl { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseAuthentication { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime? LastEmailDateTime { get; set; }

    }
}