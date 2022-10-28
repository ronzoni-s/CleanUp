using CleanUp.Application.Models;
using CleanUp.Application.Requests;
using CleanUp.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace CleanUp.Application.Interfaces
{
    public interface IPushNotificationService
    {
        Task Send(string userId, string title, string message);
    }
}
