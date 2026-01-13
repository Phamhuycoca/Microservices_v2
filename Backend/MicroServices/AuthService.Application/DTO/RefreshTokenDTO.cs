using AuthService.Domain.Entites;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.DTO
{
    public class RefreshTokenDTO
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public Guid nguoi_dung_id { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedByIp { get; set; }
    }
    public class TokenResponseDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int expires_in { get; set; }
    }
    public class RefreshTokenProfile : Profile
    {
        public RefreshTokenProfile()
        {
            CreateMap<refresh_token, RefreshTokenDTO>();
            CreateMap<RefreshTokenDTO, refresh_token>();
            CreateMap<refresh_token, TokenResponseDto>();
            CreateMap<TokenResponseDto, refresh_token>();
        }
    }
}
