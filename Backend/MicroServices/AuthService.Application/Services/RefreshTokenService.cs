using AuthService.Application.DTO;
using AuthService.Application.IServices;
using AuthService.Domain.Entites;
using AuthService.Infrastructure.Configs;
using AuthService.Infrastructure.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly UserManager<nguoi_dung> _userManager;
        private readonly IRefreshTokenRepo _repo;
        private readonly JwtOptions _jwt;
        public RefreshTokenService(UserManager<nguoi_dung> userManager
            ,IRefreshTokenRepo repo
            ,IOptions<JwtOptions> jwtOptions)
        {
            _userManager = userManager;
            _repo = repo;
            _jwt = jwtOptions.Value;
        }
        public async Task<TokenResponseDto> GennerRefreshToken(Guid nguoi_dung_id, string ip)
        {
            try
            {
                var accessTokenExpires = DateTime.UtcNow.AddMinutes(_jwt.AccessTokenMinutes);
                var user = _userManager.Users.FirstOrDefault(x => x.Id == nguoi_dung_id);
                if(user == null)
                {
                    throw new Exception("Không tìm thấy người dùng");
                }
                var accessToken = await GenerateAccessToken(user, accessTokenExpires);

                // 2. Refresh Token
                var refreshToken = GenerateRefreshToken();

                // 3. Lưu DB
                var entity = new refresh_token
                {
                    Id = Guid.NewGuid(),
                    Token = refreshToken,
                    nguoi_dung_id = nguoi_dung_id,
                    ExpiresAt = DateTime.UtcNow.AddDays(_jwt.RefreshTokenDays),
                    CreatedAt = DateTime.UtcNow,
                    CreatedByIp = ip,
                    IsRevoked = false
                };

                _repo.Add(entity);

                return new TokenResponseDto
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    AccessTokenExpiresAt = accessTokenExpires
                };
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        private async Task<string> GenerateAccessToken(
        nguoi_dung user,
        DateTime expires)
        {
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwt.SecretKey)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
    }
}
