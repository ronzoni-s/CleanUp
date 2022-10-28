using CleanUp.Application;
using CleanUp.Application.Interfaces;
using CleanUp.Application.Interfaces.Repositorys;
using CleanUp.Application.Models;
using CleanUp.Application.Requests;
using CleanUp.Domain.Entities;
using fbognini.Core.Exceptions;
using fbognini.Notifications.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CleanUp.Infrastructure.Services
{
    public class FirebasePushNotificationService : IPushNotificationService
    {
        public FirebasePushNotificationService()
        {
        }

        public async Task Send(string userId, string title, string message)
        {
            
        }
    }
}
