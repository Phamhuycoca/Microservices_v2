using AuthService.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.IServices
{
    public interface IAuthenService
    {
        Task<TokenResponseDto> Login(LoginDTO dto);
    }
}
