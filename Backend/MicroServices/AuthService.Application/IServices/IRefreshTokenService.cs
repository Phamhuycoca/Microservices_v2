using AuthService.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.IServices
{
    public interface IRefreshTokenService
    {
        Task<TokenResponseDto> GennerRefreshToken(Guid nguoi_dung_id, string ip);
    }
}
