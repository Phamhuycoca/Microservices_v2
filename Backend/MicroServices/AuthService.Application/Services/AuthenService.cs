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
        private readonly SignInManager<nguoi_dung> _signInManager;
        private readonly UserManager<nguoi_dung> _userManager;
        private readonly IRefreshTokenService _refreshTokenService;
        public AuthenService(ApplicationDbContext context
            , IRefreshTokenRepo repo
            , SignInManager<nguoi_dung> signInManager
            , UserManager<nguoi_dung> userManager
            , IMapper mapper
            , IOptions<JwtOptions> jwtOptions
            , IRefreshTokenService refreshTokenService)
        {
            _context = context;
            _repo = repo;
            _mapper = mapper;
            _signInManager = signInManager;
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
                    throw new Exception("Tài khoản đã bị khóa, vui lòng thử lại sau");
                }
                var result = await _signInManager.PasswordSignInAsync(
                user,
                dto.password,
                dto.remember_me,
                lockoutOnFailure: true);
                if (!result.Succeeded)
                {
                    var maxAttempts = _userManager.Options.Lockout.MaxFailedAccessAttempts;
                    var failedCount = await _userManager.GetAccessFailedCountAsync(user);
                    var remain = maxAttempts - failedCount;

                    if (remain > 0 && remain < 5)
                    {
                       throw new Exception($"Sai tài khoản hoặc mật khẩu. Bạn còn {remain} lần thử.");
                    }
                    else
                    {
                        throw new Exception("Tài khoản đã bị khóa do nhập sai quá nhiều lần");
                    }
                }
                return await _refreshTokenService.GennerRefreshToken(user.Id, "122345");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
