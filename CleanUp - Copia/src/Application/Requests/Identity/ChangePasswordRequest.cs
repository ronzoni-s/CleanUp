﻿using System.ComponentModel.DataAnnotations;

namespace ErbertPranzi.Application.Requests.Identity
{
    public class ChangePasswordRequest
    {
        [Required]
        public string Password { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        public string ConfirmNewPassword { get; set; }
    }
}