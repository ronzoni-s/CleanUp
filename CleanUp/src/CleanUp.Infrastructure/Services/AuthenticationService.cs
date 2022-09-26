using CleanUp.Application;
using CleanUp.Application.Common.Interfaces;
using CleanUp.Application.Common.Interfaces.Repositorys;
using CleanUp.Application.Common.Models;
using CleanUp.Application.Common.Requests;
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
    public class AuthenticationService : IAuthenticationService
    {

        private readonly UserManager<CleanUpUser> userManager;
        private readonly RoleManager<CleanUpRole> roleManager;
        private readonly AuthenticationSettings appConfig;
        private readonly ILogger<AuthenticationService> logger;

        public AuthenticationService(
            UserManager<CleanUpUser> userManager
            , RoleManager<CleanUpRole> roleManager
            , ILogger<AuthenticationService> logger
            , IOptions<AuthenticationSettings> appConfig
            )
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logger = logger;
            this.appConfig = appConfig.Value;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                throw new NotFoundException("Email o password errati");
            }
            if (!user.EmailConfirmed)
            {
                throw new NotFoundException("Account non verificato");
            }
            var passwordValid = await userManager.CheckPasswordAsync(user, model.Password);
            if (!passwordValid)
            {
                throw new NotFoundException("Email o password errati");
            }

            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await userManager.UpdateAsync(user);

            var token = await GenerateJwtAsync(user);
            return new LoginResponse 
            { 
                Token = token, 
                RefreshToken = user.RefreshToken,
                RefreshTokenExpiryTime = user.RefreshTokenExpiryTime,
                UserId = user.Id
            };
        }

        public async Task<LoginResponse> GetRefreshTokenAsync(RefreshLoginRequest model)
        {
            if (model is null)
            {
                throw new BadRequestException("Token non valido");
            }
            var userPrincipal = GetPrincipalFromExpiredToken(model.Token);
            var userEmail = userPrincipal.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(userEmail);
            if (user == null)
                throw new BadRequestException("Utente non trovato");

            if (user.RefreshToken != model.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                throw new BadRequestException("Token non valido");
            
            var token = GenerateEncryptedToken(GetSigningCredentials(), await GetClaimsAsync(user));
            user.RefreshToken = GenerateRefreshToken();
            await userManager.UpdateAsync(user);

            return new LoginResponse 
            { 
                Token = token, 
                RefreshToken = user.RefreshToken, 
                RefreshTokenExpiryTime = user.RefreshTokenExpiryTime,
                UserId = user.Id
            };
        }

        private async Task<string> GenerateJwtAsync(CleanUpUser user)
        {
            var token = GenerateEncryptedToken(GetSigningCredentials(), await GetClaimsAsync(user));
            return token;
        }

        private async Task<IEnumerable<Claim>> GetClaimsAsync(CleanUpUser user)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            var permissionClaims = new List<Claim>();
            foreach (var role in roles)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role));
                var thisRole = await roleManager.FindByNameAsync(role);
                var allPermissionsForThisRoles = await roleManager.GetClaimsAsync(thisRole);
                permissionClaims.AddRange(allPermissionsForThisRoles);
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Name, user.FirstName),
                new(ClaimTypes.Surname, user.LastName),
                new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty)
            }
            .Union(userClaims)
            .Union(roleClaims)
            .Union(permissionClaims);

            return claims;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
        {
            var token = new JwtSecurityToken(
               claims: claims,
               expires: DateTime.UtcNow.AddDays(2),
               signingCredentials: signingCredentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            var encryptedToken = tokenHandler.WriteToken(token);
            return encryptedToken;
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appConfig.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RoleClaimType = ClaimTypes.Role,
                ClockSkew = TimeSpan.Zero
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Token non valido");
            }

            return principal;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var secret = Encoding.UTF8.GetBytes(appConfig.Secret);
            return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
        }
    }
}
