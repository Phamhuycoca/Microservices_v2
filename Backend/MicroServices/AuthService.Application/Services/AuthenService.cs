using AuthService.Application.DTO;
using AuthService.Application.IServices;
using AuthService.Domain.Entites;
using AuthService.Infrastructure.AppContext;
using AuthService.Infrastructure.Configs;
using AuthService.Infrastructure.IRepositories;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Services
{
    public class AuthenService : IAuthenService
    {
        private readonly ApplicationDbContext _context;
        public readonly IRefreshTokenRepo _repo;
        private readonly JwtOptions _jwt;
        private readonly IMapper _mapper;
        private readonly UserManager<nguoi_dung> _userManager;
        private readonly IRefreshTokenService _refreshTokenService;
        public AuthenService(ApplicationDbContext context
            , IRefreshTokenRepo repo
            , UserManager<nguoi_dung> userManager
            , IMapper mapper
            , IOptions<JwtOptions> jwtOptions
            , IRefreshTokenService refreshTokenService)
        {
            _context = context;
            _repo = repo;
            _mapper = mapper;
            _userManager = userManager;
            _jwt = jwtOptions.Value;
            _refreshTokenService = refreshTokenService;
        }
        public async Task<TokenResponseDto> Login(LoginDTO dto)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(dto.user_name);
                if (user == null)
                {
                    throw new Exception("Sai tài khoản hoặc mật khẩu");
                }
                if (await _userManager.IsLockedOutAsync(user))
                {
                    throw new AppException(HttpStatusCode.Forbidden,
                       "Tài khoản đã bị khóa do nhập sai quá nhiều lần, vui lòng thử lại sau"
                   );
                }
                var result = await _userManager.CheckPasswordAsync(user, dto.password);

                if (!result)
                {
                    await _userManager.AccessFailedAsync(user);

                    var failedCount = await _userManager.GetAccessFailedCountAsync(user);
                    var maxAttempts = _userManager.Options.Lockout.MaxFailedAccessAttempts;
                    var remain = maxAttempts - failedCount;

                    if (remain > 0)
                        throw new AppException(HttpStatusCode.Unauthorized,
                            $"Sai tài khoản hoặc mật khẩu. Bạn còn {remain} lần thử."
                        );

                   
                }
                await _userManager.ResetAccessFailedCountAsync(user);
                return await _refreshTokenService.GennerRefreshToken(user.Id, "122345");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
